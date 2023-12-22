using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IAccountService
{
    Task<PaginationResult<Account>> GetAccount(
        Guid? guid = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageSize = 1,
        int pageIndex = 0
    );
    Task<Account> GetAccountById(Guid id);
}
