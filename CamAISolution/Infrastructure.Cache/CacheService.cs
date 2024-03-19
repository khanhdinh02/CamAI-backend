using Core.Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

public class CacheService(IMemoryCache cache) : ICacheService
{
    public T? Get<T>(string key)
    {
        _ = cache.TryGetValue<T>(key, out var value);
        return value;
    }

    public T Set<T>(string key, T value, TimeSpan expired)
    {
        return cache.Set(key, value, expired);
    }
}