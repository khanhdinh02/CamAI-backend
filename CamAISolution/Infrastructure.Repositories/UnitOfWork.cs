using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Repositories.Base;
using Infrastructure.Repositories.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public class UnitOfWork(CamAIContext context, IServiceProvider serviceProvider) : IUnitOfWork
{
    private bool disposed = false;
    private bool haveTransaction = false;

    public IRepository<Shop> Shops => serviceProvider.GetRequiredService<IRepository<Shop>>();

    public IRepository<Ward> Wards => serviceProvider.GetRequiredService<IRepository<Ward>>();

    public IRepository<ShopStatus> ShopStatuses => serviceProvider.GetRequiredService<IRepository<ShopStatus>>();

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
}
