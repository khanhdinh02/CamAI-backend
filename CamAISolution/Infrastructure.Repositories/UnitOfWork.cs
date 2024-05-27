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

    public IRepository<Brand> Brands => serviceProvider.GetRequiredService<IRepository<Brand>>();

    public ICustomShopRepository Shops => serviceProvider.GetRequiredService<ICustomShopRepository>();

    public IRepository<Province> Provinces => serviceProvider.GetRequiredService<IRepository<Province>>();
    public IRepository<District> Districts => serviceProvider.GetRequiredService<IRepository<District>>();
    public IRepository<Ward> Wards => serviceProvider.GetRequiredService<IRepository<Ward>>();

    public ICustomAccountRepository Accounts => serviceProvider.GetRequiredService<ICustomAccountRepository>();
    public ICustomEmployeeRepository Employees => serviceProvider.GetRequiredService<ICustomEmployeeRepository>();
    public IRepository<EdgeBox> EdgeBoxes => serviceProvider.GetRequiredService<IRepository<EdgeBox>>();
    public ICustomEdgeBoxActivityRepository EdgeBoxActivities =>
        serviceProvider.GetRequiredService<ICustomEdgeBoxActivityRepository>();
    public ICustomEdgeBoxInstallRepository EdgeBoxInstalls =>
        serviceProvider.GetRequiredService<ICustomEdgeBoxInstallRepository>();
    public IRepository<EdgeBoxModel> EdgeBoxModels => serviceProvider.GetRequiredService<IRepository<EdgeBoxModel>>();
    public ICustomIncidentRepository Incidents => serviceProvider.GetRequiredService<ICustomIncidentRepository>();
    public IRepository<Evidence> Evidences => serviceProvider.GetRequiredService<IRepository<Evidence>>();
    public IRepository<Camera> Cameras => serviceProvider.GetRequiredService<IRepository<Camera>>();
    public IRepository<SupervisorAssignment> SupervisorAssignments =>
        serviceProvider.GetRequiredService<IRepository<SupervisorAssignment>>();

    public Task BeginTransaction()
    {
        haveTransaction = true;
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransaction()
    {
        if (haveTransaction)
        {
            UpdateAuditableEntities();
            return context.Database.CommitTransactionAsync();
        }
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
        UpdateAuditableEntities();
        return await context.SaveChangesAsync();
    }

    private void UpdateAuditableEntities()
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
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.ModifiedTime = DateTimeHelper.VNDateTime;
                try
                {
                    entry.Entity.ModifiedById = serviceProvider.GetRequiredService<IJwtService>().GetCurrentUser().Id;
                }
                catch (Exception)
                {
                    entry.Entity.ModifiedById = null;
                }
            }
        }
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

    public void Detach(object entity)
    {
        context.Entry(entity).State = EntityState.Detached;
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
