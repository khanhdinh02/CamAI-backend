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
    ICustomEdgeBoxInstallRepository EdgeBoxInstalls { get; }
    IRepository<EdgeBoxModel> EdgeBoxModels { get; }
    ICustomIncidentRepository Incidents { get; }
    IRepository<Evidence> Evidences { get; }
    IRepository<Camera> Cameras { get; }
    ICustomEdgeBoxActivityRepository EdgeBoxActivities { get; }
    IRepository<SupervisorAssignment> SupervisorAssignments { get; }
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBack();
    int Complete();
    Task<int> CompleteAsync();

    IRepository<T> GetRepository<T>();
    void Detach(object entity);
}
