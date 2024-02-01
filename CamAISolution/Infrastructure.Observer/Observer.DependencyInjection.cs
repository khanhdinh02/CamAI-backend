using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Observer;

public static class ObserverDependencyInjection
{
    public static IServiceCollection AddObserver(this IServiceCollection services)
    {
        services.AddSingleton<SyncObserver>();
        return services;
    }
}
