using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Repositories.Base;
using Infrastructure.Repositories.Data;

namespace Infrastructure.Repositories;

public class UnitOfWork(CamAIContext context, IRepository<Shop> shops, IRepository<Ward> wards) : IUnitOfWork
{
    private bool disposed = false;

    public IRepository<Shop> Shops => shops;

    public IRepository<Ward> Wards => wards;

    public Task BeginTransaction()
    {
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransaction()
    {
        return context.Database.CommitTransactionAsync();
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
