using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories.Base;

namespace Core.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Brand> Brands { get; }
    IRepository<Shop> Shops { get; }
    IRepository<Ward> Wards { get; }
    IRepository<ShopStatus> ShopStatuses { get; }
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBack();
    int Complete();
    Task<int> CompleteAsync();
}
