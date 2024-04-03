using Core.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Streaming;

public static class DependencyInjection
{
    public static IServiceCollection AddStreaming(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StreamingConfiguration>(configuration.GetRequiredSection("Streaming"));
        WebsocketRelayProcess.Configuration = configuration
            .GetRequiredSection("Streaming")
            .Get<StreamingConfiguration>()!;
        services.AddScoped<IStreamingService, StreamingService>();
        return services;
    }
}
