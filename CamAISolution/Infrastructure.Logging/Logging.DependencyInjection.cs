using Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Logging;

public static class LoggingDependencyInjection
{
    public static IServiceCollection AddLoggingDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IAppLogging<>), typeof(AppLogging<>));
        return services;
    }
}
