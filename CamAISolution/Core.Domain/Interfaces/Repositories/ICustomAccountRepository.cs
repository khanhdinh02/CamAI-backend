using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Repositories;

public interface ICustomAccountRepository : IRepository<Account>
{
    void UpdateStatusInBrand(Guid brandId, AccountStatus status);
}
