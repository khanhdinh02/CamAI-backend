using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AccountService(IUnitOfWork unitOfWork, IJwtService jwtService, IBaseMapping mapper) : IAccountService
{
    public Task<PaginationResult<Account>> GetAccount(
        Guid? guid = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageSize = 1,
        int pageIndex = 0
    )
    {
        /* var specification = new AccountSearchSpecification(guid, from, to, pageSize, pageIndex);
         return accountRepo.GetAsync(specification);*/
        throw new NotImplementedException();
        //return accountRepo.GetAsync(specification);
    }

    public async Task<Account> GetAccountById(Guid id)
    {
        var foundAccounts = await unitOfWork.Accounts.GetAsync(new AccountByIdRepoSpec(id));
        if (foundAccounts.Values.Count == 0)
            throw new NotFoundException(typeof(Account), id);
        return foundAccounts.Values[0];
    }

    public Task<Account> GetCurrentAccount()
    {
        return GetAccountById(jwtService.GetCurrentUserId());
    }

    public async Task<Account> CreateAccount(CreateAccountDto dto)
    {
        if (await unitOfWork.Accounts.CountAsync(a => a.Email == dto.Email) > 0)
            throw new BadRequestException("Email is already taken");
        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);
        dto.Password = Hasher.Hash(dto.Password);
        var currentUser = await GetCurrentAccount();

        if (currentUser.HasRole(RoleEnum.Admin))
        {
            if (dto.RoleIds.Any(r => r == RoleEnum.BrandManager))
                return await CreateBrandManager(dto);
            if (dto.RoleIds.Any(r => r == RoleEnum.Technician))
                return await CreateTechnician(dto);
            throw new BadRequestException("Admin can only create brand manager or technician");
        }

        if (currentUser.HasRole(RoleEnum.BrandManager))
        {
            if (dto.RoleIds.Any(r => r == RoleEnum.ShopManager))
                return await CreateShopManager(dto);
            if (dto.RoleIds.Any(r => r == RoleEnum.Employee))
            {
                // TODO: Create employee
                throw new NotImplementedException();
            }
            throw new BadRequestException("Brand manager can only create shop manager or employee");
        }

        // Other roles that can create account

        return null!;
    }

    private async Task<Account> CreateBrandManager(CreateAccountDto dto)
    {
        var newAccount = mapper.Map<CreateAccountDto, Account>(dto);
        // If brandId is null, throw exception
        if (dto.BrandId == null)
            throw new BadRequestException("BrandId is required");

        // If brandId is not null, check if brand exists
        var brand =
            await unitOfWork.Brands.GetByIdAsync(dto.BrandId.Value)
            ?? throw new NotFoundException(typeof(Brand), dto.BrandId.Value);

        // If brand exists, check if brand manager exists
        if (brand.BrandManagerId != null)
            throw new BadRequestException("Brand manager already exists");

        newAccount.WorkingShopId = null;
        newAccount.Brand = brand;
        newAccount.Roles = [await unitOfWork.Roles.GetByIdAsync(RoleEnum.BrandManager)];
        newAccount.AccountStatusId = AccountStatusEnum.New;
        await unitOfWork.Accounts.AddAsync(newAccount);
        await unitOfWork.CompleteAsync();
        return newAccount;
    }

    private async Task<Account> CreateTechnician(CreateAccountDto dto)
    {
        var newAccount = mapper.Map<CreateAccountDto, Account>(dto);
        newAccount.WorkingShopId = null;
        newAccount.Roles = [await unitOfWork.Roles.GetByIdAsync(RoleEnum.Technician)];
        newAccount.AccountStatusId = AccountStatusEnum.New;
        await unitOfWork.Accounts.AddAsync(newAccount);
        await unitOfWork.CompleteAsync();
        return newAccount;
    }

    private async Task<Account> CreateShopManager(CreateAccountDto dto)
    {
        var newAccount = mapper.Map<CreateAccountDto, Account>(dto);
        // If workingShopId is null, throw exception
        if (dto.WorkingShopId == null)
            throw new BadRequestException("WorkingShopId is required");

        // If workingShopId is not null, check if shop exists
        var shop =
            await unitOfWork.Shops.GetByIdAsync(dto.WorkingShopId.Value)
            ?? throw new NotFoundException(typeof(Shop), dto.WorkingShopId.Value);

        // If shop exist, check if shop is in brand of current user
        var currentUser = await GetCurrentAccount();
        if (!currentUser.HasRole(RoleEnum.BrandManager) || currentUser.Brand?.Id != shop.BrandId)
            throw new BadRequestException("Shop is not in brand of current user");

        // If shop exists, check if shop manager exists
        if (shop.ShopManagerId != null)
            throw new BadRequestException("Shop manager already exists");

        newAccount.WorkingShopId = null;
        newAccount.ManagingShop = shop;
        newAccount.Roles = [await unitOfWork.Roles.GetByIdAsync(RoleEnum.ShopManager)];
        newAccount.AccountStatusId = AccountStatusEnum.New;
        await unitOfWork.Accounts.AddAsync(newAccount);
        await unitOfWork.CompleteAsync();
        return newAccount;
    }
}
