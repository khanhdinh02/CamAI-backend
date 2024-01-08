using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IAccountService
{
    Task<Account> CreateAccount(CreateAccountDto dto);
    Task<PaginationResult<Account>> GetAccounts(SearchAccountRequest req);
    Task<Account> GetAccountById(Guid id);
    Task<Account> GetCurrentAccount();
}
