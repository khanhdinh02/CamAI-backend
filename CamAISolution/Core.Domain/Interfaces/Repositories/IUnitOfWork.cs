using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Shop> Shops { get; }
    IRepository<Ward> Wards { get; }
    IRepository<ShopStatus> ShopStatuses { get; }
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBack();
    int Complete();
    Task<int> CompleteAsync();
}
