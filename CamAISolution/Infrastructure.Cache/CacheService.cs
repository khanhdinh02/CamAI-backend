using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

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
        var expirationToken = new CancellationChangeToken(new CancellationTokenSource(expired).Token);
        var memoryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.UtcNow.Add(expired))
            //Force eviction run
            .AddExpirationToken(expirationToken);

        return cache.Set(key, value, memoryOptions);
    }

    public T Set<T>(string key, T value, TimeSpan expired, Action<string, object?> postEvictionCallBack)
    {
        var expirationToken = new CancellationChangeToken(new CancellationTokenSource(expired).Token);
        var memoryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.UtcNow.Add(expired))
            // Force eviction run
            .AddExpirationToken(expirationToken)
            .RegisterPostEvictionCallback((key, value, _, _) =>
            {
                postEvictionCallBack((string)key, value);
            }, cache);
        return cache.Set(key, value, memoryOptions);
    }

    public void Remove(string key) => cache.Remove(key);

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
