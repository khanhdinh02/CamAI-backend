using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Infrastructure.Observer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Observer;

public static class ObserverDependencyInjection
{
    public static IServiceCollection AddObserver(this IServiceCollection services, IConfiguration configuration)
    {
        var aiConfiguration = configuration.GetRequiredSection("Ai").Get<AiConfiguration>()!;
        services.AddScoped<IMessageQueueService, MessageQueueService>();
        services.AddSingleton(aiConfiguration);
        services.AddSingleton<SyncObserver>();
        services.AddSingleton<HumanCountFileSaverObserver>();
        return services;
    }
}