using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Role> Roles { get; }
    IRepository<Brand> Brands { get; }
    IRepository<Shop> Shops { get; }
    IRepository<Ward> Wards { get; }
    IRepository<ShopStatus> ShopStatuses { get; }
    IRepository<Account> Accounts { get; }
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBack();
    int Complete();
    Task<int> CompleteAsync();
}
