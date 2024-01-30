using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Role> Roles { get; }
    IRepository<Brand> Brands { get; }
    IRepository<Shop> Shops { get; }
    IRepository<Province> Provinces { get; }
    IRepository<District> Districts { get; }
    IRepository<Ward> Wards { get; }
    IRepository<ShopStatus> ShopStatuses { get; }
    IRepository<Account> Accounts { get; }
    IRepository<Employee> Employees { get; }
    IRepository<Ticket> Tickets { get; }
    IRepository<TicketStatus> TicketStatuses { get; }
    IRepository<TicketType> TicketTypes { get; }
    IRepository<EdgeBox> EdgeBoxes { get; }
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBack();
    int Complete();
    Task<int> CompleteAsync();

    IRepository<T> GetRepository<T>();
}
