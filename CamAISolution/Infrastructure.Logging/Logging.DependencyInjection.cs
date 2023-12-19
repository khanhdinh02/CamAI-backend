using Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging;

public static class LoggingDependencyInjection
{
    public static IServiceCollection AddLoggingDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IAppLogging<>), typeof(AppLogging<>));
        return services;
    }
}
