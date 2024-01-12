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

// TODO [Khanh]: What authority does shop manager have over employees?
// This affects the implementation of CreateAccount, UpdateAccount, DeleteAccount
public class AccountService(IUnitOfWork unitOfWork, IJwtService jwtService, IBaseMapping mapper) : IAccountService
{
    public async Task<PaginationResult<Account>> GetAccounts(SearchAccountRequest req)
    {
        var user = await GetCurrentAccount();
        if (!user.HasRole(RoleEnum.Admin))
        {
            if (user.HasRole(RoleEnum.BrandManager))
                req.BrandId = user.Brand?.Id;
            else if (user.HasRole(RoleEnum.ShopManager))
                req.ShopId = user.ManagingShop?.Id;
        }
        var accounts = await unitOfWork.Accounts.GetAsync(new AccountSearchSpec(req, user));
        return accounts;
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
        // Allow to create account with the same email if the account was deleted (inactive)
        // This will override the old account, but the id and old records (violation, request…) will be kept
        Account newAccount;
        var newEntity = true;
        var accountThatHasTheSameMail = (
            await unitOfWork
                .Accounts
                .GetAsync(
                    a => a.Email == dto.Email,
                    includeProperties: [nameof(Account.ManagingShop), nameof(Account.Roles)],
                    disableTracking: false,
                    orderBy: o => o.OrderBy(a => a.Id) // TODO: Remove this line after the bug is fixed
                )
        )
            .Values
            .FirstOrDefault();
        if (accountThatHasTheSameMail == null)
            newAccount = mapper.Map<CreateAccountDto, Account>(dto);
        else
        {
            newEntity = false;
            if (accountThatHasTheSameMail.AccountStatusId != AccountStatusEnum.Inactive)
                throw new BadRequestException("Email is already taken");
            accountThatHasTheSameMail.BrandId = null;
            accountThatHasTheSameMail.WorkingShopId = null;
            accountThatHasTheSameMail.ManagingShop = null;

            newAccount = mapper.Map(dto, accountThatHasTheSameMail);
        }

        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);
        newAccount.Password = Hasher.Hash(dto.Password);
        var currentUser = await GetCurrentAccount();

        if (currentUser.HasRole(RoleEnum.Admin))
        {
            if (dto.RoleIds.Any(r => r == RoleEnum.BrandManager))
                newAccount = await CreateBrandManager(newAccount);
            else if (dto.RoleIds.Any(r => r == RoleEnum.Technician))
                newAccount = await CreateTechnician(newAccount);
            else
                throw new BadRequestException("Admin can only create brand manager or technician");
        }
        else if (currentUser.HasRole(RoleEnum.BrandManager))
        {
            if (dto.RoleIds.Any(r => r == RoleEnum.ShopManager))
            {
                dto.BrandId = currentUser.Brand?.Id;
                newAccount = await CreateShopManager(newAccount);
            }
            else if (dto.RoleIds.Any(r => r == RoleEnum.Employee))
            {
                // TODO [Khanh]: Create employee
                throw new NotImplementedException();
            }
            else
                throw new BadRequestException("Brand manager can only create shop manager or employee");
        }

        // Other roles that can create account

        if (newEntity)
            await unitOfWork.Accounts.AddAsync(newAccount);
        else
            unitOfWork.Accounts.Update(newAccount);
        await unitOfWork.CompleteAsync();
        return newAccount;
    }

    public async Task<Account> UpdateAccount(Guid id, UpdateAccountDto dto)
    {
        // TODO: Update role. Currently, the role can only be changed by deleting and recreating the account
        var account =
            (await unitOfWork.Accounts.GetAsync(new AccountByIdRepoSpec(id))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Account), id);
        if (dto.Email != account.Email && await unitOfWork.Accounts.CountAsync(a => a.Email == dto.Email) > 0)
            throw new BadRequestException("Email is already taken");
        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);

        mapper.Map(dto, account);
        var user = await GetCurrentAccount();
        if (user.HasRole(RoleEnum.Admin))
        {
            if (account.HasRole(RoleEnum.BrandManager) || account.HasRole(RoleEnum.Technician))
                unitOfWork.Accounts.Update(account);
            else
                throw new BadRequestException("Admin can only update brand manager or technician");
        }
        else if (user.HasRole(RoleEnum.BrandManager))
        {
            if (account.HasRole(RoleEnum.ShopManager) || account.HasRole(RoleEnum.Employee))
                unitOfWork.Accounts.Update(account);
            else
                throw new BadRequestException("Brand manager can only update shop manager or employee");
        }

        await unitOfWork.CompleteAsync();
        return account;
    }

    private async Task<Account> CreateBrandManager(Account newAccount)
    {
        if (newAccount.BrandId == null)
            throw new BadRequestException("BrandId is required");

        var brand =
            await unitOfWork.Brands.GetByIdAsync(newAccount.BrandId.Value)
            ?? throw new NotFoundException(typeof(Brand), newAccount.BrandId.Value);

        if (
            await unitOfWork
                .Accounts
                .CountAsync(
                    a => a.Roles.Contains(new Role { Id = RoleEnum.BrandManager }) && a.BrandId == newAccount.BrandId
                ) > 0
        )
            throw new BadRequestException("Brand manager already exists");

        newAccount.Brand = brand;
        newAccount.Roles = [await unitOfWork.Roles.GetByIdAsync(RoleEnum.BrandManager)];
        newAccount.AccountStatusId = AccountStatusEnum.New;
        return newAccount;
    }

    private async Task<Account> CreateTechnician(Account newAccount)
    {
        newAccount.Roles = [await unitOfWork.Roles.GetByIdAsync(RoleEnum.Technician)];
        newAccount.AccountStatusId = AccountStatusEnum.New;
        return newAccount;
    }

    private async Task<Account> CreateShopManager(Account newAccount)
    {
        newAccount.Roles = [await unitOfWork.Roles.GetByIdAsync(RoleEnum.ShopManager)];
        newAccount.AccountStatusId = AccountStatusEnum.New;
        return newAccount;
    }
}
