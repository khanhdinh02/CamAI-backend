using Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache;

public static class CachingDependencyInjection
{
    public static IServiceCollection AddCacheService(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }
}
