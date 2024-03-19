using Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache;

public static class CachingDependencyInjection
{
    public static IServiceCollection AddCacheService(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, CacheService>();
        return services;
    }
}