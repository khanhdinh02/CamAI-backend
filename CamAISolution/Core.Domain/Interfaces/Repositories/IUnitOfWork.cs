using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Brand> Brands { get; }
    IRepository<Shop> Shops { get; }
    IRepository<Province> Provinces { get; }
    IRepository<District> Districts { get; }
    IRepository<Ward> Wards { get; }
    IRepository<Account> Accounts { get; }
    IRepository<Employee> Employees { get; }
    IRepository<EdgeBox> EdgeBoxes { get; }
    IRepository<EdgeBoxInstall> EdgeBoxInstalls { get; }
    IRepository<EdgeBoxModel> EdgeBoxModels { get; }
    IRepository<Incident> Incidents { get; }
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBack();
    int Complete();
    Task<int> CompleteAsync();

    IRepository<T> GetRepository<T>();
}
