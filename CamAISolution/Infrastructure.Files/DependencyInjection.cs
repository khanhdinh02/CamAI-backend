using Core.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Files;

public static class DependencyInjection
{
    public static IServiceCollection AddReadFile(this IServiceCollection services)
    {
        services.AddScoped<IReadFileService, ReadFileService>();
        return services;
    }
}