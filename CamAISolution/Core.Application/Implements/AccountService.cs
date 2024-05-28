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

    public async Task<Account> GetAccountById(Guid id, bool includeAdmin = false)
    {
        var foundAccounts = await unitOfWork.Accounts.GetAsync(new AccountByIdRepoSpec(id));
        if (foundAccounts.Values.Count == 0 || (!includeAdmin && foundAccounts.Values[0].Role == Role.Admin))
            throw new NotFoundException(typeof(Account), id);
        return foundAccounts.Values[0];
    }

    public Account GetCurrentAccount() => jwtService.GetCurrentUser();

    public async Task<Account> CreateAccount(CreateAccountDto dto)
    {
        // TODO: Reconsider the logic below in the future
        // Allow to create account with the same email if the account was deleted (inactive)
        // This will override the old account, but the id and old records (violation, request...) will be kept
        Account newAccount;
        var newEntity = true;
        var accountThatHasTheSameMail = (
            await unitOfWork.Accounts.GetAsync(
                a => a.Email == dto.Email,
                includeProperties: [nameof(Account.ManagingShop)],
                disableTracking: false
            )
        ).Values.FirstOrDefault();
        if (accountThatHasTheSameMail == null)
            newAccount = mapper.Map<CreateAccountDto, Account>(dto);
        else
        {
            newEntity = false;
            var employee = (await unitOfWork.Employees.GetAsync(x => x.Email == dto.Email)).Values.FirstOrDefault();
            if (
                accountThatHasTheSameMail.AccountStatus != AccountStatus.Inactive
                || (employee != null && employee.EmployeeStatus != EmployeeStatus.Inactive)
            )
                throw new BadRequestException("Email is already taken");
            accountThatHasTheSameMail.BrandId = null;
            accountThatHasTheSameMail.ManagingShop = null;

            newAccount = mapper.Map(dto, accountThatHasTheSameMail);
        }

        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);
        newAccount.Password = Hasher.Hash(DomainHelper.GenerateDefaultPassword(dto.Email));
        var currentUser = GetCurrentAccount();

        if (currentUser.Role == Role.Admin)
        {
            if (dto.Role == Role.BrandManager)
                newAccount = await CreateBrandManager(newAccount);
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
        var account =
            (await unitOfWork.Accounts.GetAsync(new AccountByIdRepoSpec(id))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Account), id);
        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);

        // TODO: update employee as well
        mapper.Map(dto, account);
        var user = GetCurrentAccount();
        switch (user.Role)
        {
            case Role.Admin when account.Role is Role.BrandManager:
            case Role.BrandManager when account.Role == Role.ShopManager:
                unitOfWork.Accounts.Update(account);
                break;
            default:
                throw new ForbiddenException(user, account);
        }

        await unitOfWork.CompleteAsync();
        return account;
    }

    public async Task DeleteAccount(Guid id)
    {
        var user = GetCurrentAccount();
        var account = await GetAccountById(id);
        switch (user.Role)
        {
            case Role.Admin when account.Role is not (Role.BrandManager or Role.ShopManager):
            case Role.BrandManager when account.Role != Role.ShopManager:
                throw new ForbiddenException(user, account);
        }

        if (account is { Role: Role.ShopManager, AccountStatus: AccountStatus.New })
            unitOfWork.Accounts.Delete(account);
        else
        {
            account.AccountStatus = AccountStatus.Inactive;
            unitOfWork.Accounts.Update(account);
        }
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

    public async Task<Account> CreateSupervisor(CreateSupervisorDto dto)
    {
        var employee =
            (
                await unitOfWork.Employees.GetAsync(
                    e => e.Id == dto.EmployeeId,
                    includeProperties: [nameof(Employee.Shop)],
                    disableTracking: false
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Employee), dto.EmployeeId);
        if (employee.AccountId != null)
            throw new BadRequestException("Employee already has an account");
        if (await unitOfWork.Accounts.CountAsync(a => a.Email == dto.Email) > 0)
            throw new BadRequestException("Email is already taken");

        var newAccount = mapper.Map<Employee, Account>(employee);
        newAccount.Email = dto.Email;
        newAccount.Password = Hasher.Hash(DomainHelper.GenerateDefaultPassword(dto.Email));
        newAccount.Role = Role.ShopSupervisor;
        newAccount.Employee = employee;
        newAccount.BrandId = employee.Shop?.BrandId;
        newAccount.AccountStatus = AccountStatus.New;
        newAccount = await unitOfWork.Accounts.AddAsync(newAccount);
        await unitOfWork.CompleteAsync();
        return newAccount;
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

    private Account CreateShopManager(Account newAccount)
    {
        newAccount.Role = Role.ShopManager;
        newAccount.AccountStatus = AccountStatus.New;
        return newAccount;
    }

    public async Task<Account> ActivateAccount(Guid id)
    {
        var currentAccount = GetCurrentAccount();
        var account = await GetAccountById(id);

        if (account.AccountStatus == AccountStatus.Inactive)
        {
            account.AccountStatus = AccountStatus.Active;
            unitOfWork.Accounts.Update(account);
            await unitOfWork.CompleteAsync();
        }

        return account;
    }
}
