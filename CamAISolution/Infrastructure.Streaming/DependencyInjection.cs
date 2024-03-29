using Core.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Streaming;

public static class DependencyInjection
{
    public static IServiceCollection AddStreaming(this IServiceCollection services)
    {
        services.AddScoped<IStreamingService, StreamingService>();
        return services;
    }
}
