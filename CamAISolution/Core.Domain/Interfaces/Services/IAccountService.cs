using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface IAccountService
{
    Task<Account> CreateAccount(CreateAccountDto dto);
    Task<PaginationResult<Account>> GetAccounts(SearchAccountRequest req);
    Task<Account> GetAccountById(Guid id, bool includeAdmin = false);
    Task<Account> UpdateAccount(Guid id, UpdateAccountDto dto);
    Task DeleteAccount(Guid id);
    Account GetCurrentAccount();
    Task<Account> UpdateProfile(UpdateProfileDto dto);
    Task<Account> CreateSupervisor(CreateSupervisorDto dto);
    Task<Account> ActivateAccount(Guid id);
}
