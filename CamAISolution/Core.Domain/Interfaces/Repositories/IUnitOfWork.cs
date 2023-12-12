namespace Core.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task BeginTransaction();
    Task CommitTransaction();
    int Complete();
    Task<int> CompleteAsync();
}
