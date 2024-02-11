using Core.Domain.Entities;
using Core.Domain.Entities.Base;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public class UnitOfWork(CamAIContext context, IServiceProvider serviceProvider) : IUnitOfWork
{
    private bool disposed = false;
    private bool haveTransaction = false;

    public IRepository<Role> Roles => serviceProvider.GetRequiredService<IRepository<Role>>();
    public IRepository<Brand> Brands => serviceProvider.GetRequiredService<IRepository<Brand>>();

    public IRepository<Shop> Shops => serviceProvider.GetRequiredService<IRepository<Shop>>();

    public IRepository<Province> Provinces => serviceProvider.GetRequiredService<IRepository<Province>>();
    public IRepository<District> Districts => serviceProvider.GetRequiredService<IRepository<District>>();
    public IRepository<Ward> Wards => serviceProvider.GetRequiredService<IRepository<Ward>>();

    public IRepository<ShopStatus> ShopStatuses => serviceProvider.GetRequiredService<IRepository<ShopStatus>>();

    public IRepository<Account> Accounts => serviceProvider.GetRequiredService<IRepository<Account>>();
    public IRepository<Employee> Employees => serviceProvider.GetRequiredService<IRepository<Employee>>();
    public IRepository<EdgeBox> EdgeBoxes => serviceProvider.GetRequiredService<IRepository<EdgeBox>>();

    public IRepository<Ticket> Tickets => serviceProvider.GetRequiredService<IRepository<Ticket>>();

    public IRepository<TicketStatus> TicketStatuses => serviceProvider.GetRequiredService<IRepository<TicketStatus>>();

    public IRepository<TicketType> TicketTypes => serviceProvider.GetRequiredService<IRepository<TicketType>>();

    public IRepository<Shift> Shifts => serviceProvider.GetRequiredService<IRepository<Shift>>();
    public IRepository<EmployeeShift> EmployeeShifts =>
        serviceProvider.GetRequiredService<IRepository<EmployeeShift>>();

    public Task BeginTransaction()
    {
        haveTransaction = true;
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransaction()
    {
        if (haveTransaction)
            return context.Database.CommitTransactionAsync();
        return Task.CompletedTask;
    }

    public Task RollBack()
    {
        return context.Database.RollbackTransactionAsync();
    }

    public int Complete()
    {
        return context.SaveChangesAsync().GetAwaiter().GetResult();
    }

    public async Task<int> CompleteAsync()
    {
        foreach (var entry in context.ChangeTracker.Entries<BusinessEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTimeHelper.VNDateTime;
                    entry.Entity.ModifiedDate = DateTimeHelper.VNDateTime;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedDate = DateTimeHelper.VNDateTime;
                    break;
            }
        }
        foreach (var entry in context.ChangeTracker.Entries<ActivityEntity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedTime = DateTimeHelper.VNDateTime;
                entry.Entity.ModifiedById = serviceProvider.GetRequiredService<IJwtService>().GetCurrentUser().Id;
            }
        }
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;
        if (disposing)
        {
            context.Dispose();
        }
        disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    public IRepository<T> GetRepository<T>()
    {
        return serviceProvider.GetRequiredService<IRepository<T>>();
    }
}
