using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface IAccountService
{
    Task<Account> CreateAccount(CreateAccountDto dto);
    Task<PaginationResult<Account>> GetAccount(
        Guid? guid = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageSize = 1,
        int pageIndex = 0
    );
    Task<Account> GetAccountById(Guid id);
    Task<Account> GetCurrentAccount();
}
