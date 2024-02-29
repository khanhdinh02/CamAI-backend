using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AccountService(IUnitOfWork unitOfWork, IJwtService jwtService, IBaseMapping mapper) : IAccountService
{
    public async Task<PaginationResult<Account>> GetAccounts(SearchAccountRequest req)
    {
        var user = GetCurrentAccount();
        if (user.Role == Role.BrandManager)
            req.BrandId = user.Brand?.Id;
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

    //TODO [Dat]: Make this return Account's detail
    public Account GetCurrentAccount()
    {
        return jwtService.GetCurrentUser();
    }

    public async Task<Account> CreateAccount(CreateAccountDto dto)
    {
        // TODO: Reconsider the logic below in the future
        // Allow to create account with the same email if the account was deleted (inactive)
        // This will override the old account, but the id and old records (violation, request...) will be kept
        Account newAccount;
        var newEntity = true;
        var accountThatHasTheSameMail = (
            await unitOfWork
                .Accounts
                .GetAsync(
                    a => a.Email == dto.Email,
                    includeProperties: [nameof(Account.ManagingShop)],
                    disableTracking: false
                )
        )
            .Values
            .FirstOrDefault();
        if (accountThatHasTheSameMail == null)
            newAccount = mapper.Map<CreateAccountDto, Account>(dto);
        else
        {
            newEntity = false;
            if (accountThatHasTheSameMail.AccountStatus != AccountStatus.Inactive)
                throw new BadRequestException("Email is already taken");
            accountThatHasTheSameMail.BrandId = null;
            accountThatHasTheSameMail.ManagingShop = null;

            newAccount = mapper.Map(dto, accountThatHasTheSameMail);
        }

        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);
        newAccount.Password = Hasher.Hash(dto.Password);
        var currentUser = GetCurrentAccount();

        if (currentUser.Role == Role.Admin)
        {
            if (dto.Role == Role.BrandManager)
                newAccount = await CreateBrandManager(newAccount);
            else if (dto.Role == Role.Technician)
                newAccount = CreateTechnician(newAccount);
            else
                throw new ForbiddenException(currentUser, typeof(Account));
        }
        else if (currentUser.Role == Role.BrandManager)
        {
            if (dto.Role == Role.ShopManager)
            {
                newAccount.BrandId = currentUser.Brand?.Id;
                newAccount = CreateShopManager(newAccount);
            }
            else
                throw new ForbiddenException(currentUser, typeof(Account));
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
        var user = GetCurrentAccount();
        if (user.Role == Role.Admin)
        {
            if (account.Role == Role.BrandManager || account.Role == Role.Technician)
                unitOfWork.Accounts.Update(account);
            else
                throw new ForbiddenException(user, account);
        }
        else if (user.Role == Role.BrandManager)
        {
            if (account.Role == Role.ShopManager || account.Role == Role.Employee)
                unitOfWork.Accounts.Update(account);
            else
                throw new ForbiddenException(user, account);
        }

        await unitOfWork.CompleteAsync();
        return account;
    }

    public async Task DeleteAccount(Guid id)
    {
        var user = GetCurrentAccount();
        var account = await GetAccountById(id);
        if (user.Role == Role.Admin)
        {
            if (account.Role != Role.BrandManager && account.Role != Role.Technician)
                throw new ForbiddenException(user, account);
        }
        else if (user.Role == Role.BrandManager)
        {
            if (account.Role != Role.ShopManager && account.Role != Role.Employee)
                throw new ForbiddenException(user, account);
        }

        account.AccountStatus = AccountStatus.Inactive;
        unitOfWork.Accounts.Update(account);
        await unitOfWork.CompleteAsync();
    }

    public async Task<Account> UpdateProfile(UpdateProfileDto dto)
    {
        var account = GetCurrentAccount();
        if (dto.Email != account.Email && await unitOfWork.Accounts.CountAsync(a => a.Email == dto.Email) > 0)
            throw new BadRequestException("Email is already taken");
        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);

        mapper.Map(dto, account);
        unitOfWork.Accounts.Update(account);
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
            await unitOfWork.Accounts.CountAsync(a => a.Role == Role.BrandManager && a.BrandId == newAccount.BrandId)
            > 0
        )
            throw new BadRequestException("Brand manager already exists for this brand");

        newAccount.Brand = brand;
        newAccount.ManagingBrand = brand;
        newAccount.Role = Role.BrandManager;
        newAccount.AccountStatus = AccountStatus.New;
        return newAccount;
    }

    private Account CreateTechnician(Account newAccount)
    {
        newAccount.Role = Role.Technician;
        newAccount.AccountStatus = AccountStatus.New;
        return newAccount;
    }

    private Account CreateShopManager(Account newAccount)
    {
        newAccount.Role = Role.ShopManager;
        newAccount.AccountStatus = AccountStatus.New;
        return newAccount;
    }
}
