using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Brand> Brands { get; }
    ICustomShopRepository Shops { get; }
    IRepository<Province> Provinces { get; }
    IRepository<District> Districts { get; }
    IRepository<Ward> Wards { get; }
    ICustomAccountRepository Accounts { get; }
    ICustomEmployeeRepository Employees { get; }
    IRepository<EdgeBox> EdgeBoxes { get; }
    IRepository<EdgeBoxInstall> EdgeBoxInstalls { get; }
    IRepository<EdgeBoxInstallActivity> EdgeBoxInstallActivities { get; }
    IRepository<EdgeBoxModel> EdgeBoxModels { get; }
    IRepository<Incident> Incidents { get; }
    IRepository<Evidence> Evidences { get; }
    IRepository<Request> Requests { get; }
    IRepository<Camera> Cameras { get; }
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBack();
    int Complete();
    Task<int> CompleteAsync();

    IRepository<T> GetRepository<T>();
}
