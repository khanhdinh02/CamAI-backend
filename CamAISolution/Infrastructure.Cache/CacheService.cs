using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache;

public class CacheService(IServiceProvider provider, IMemoryCache cache) : ICacheService
{
    private Mutex cacheMutex = new();

    public T? Get<T>(string key)
    {
        _ = cache.TryGetValue<T>(key, out var value);
        return value;
    }

    public T Set<T>(string key, T value, TimeSpan expired)
    {
        return cache.Set(key, value, expired);
    }

    public async Task<Guid> GetAdminAccount()
    {
        cacheMutex.WaitOne();
        var adminAccount = await cache.GetOrCreateAsync(
            "AdminAccounts",
            async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                using var scope = provider.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                return (await uow.Accounts.GetAsync(expression: a => a.Role == Role.Admin, takeAll: true)).Values[0];
            }
        );
        cacheMutex.ReleaseMutex();

        return adminAccount!.Id;
    }
}
