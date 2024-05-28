namespace Infrastructure.Blob;

using Core.Application.Implements;
using Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBlobService(this IServiceCollection services)
    {
        services.AddScoped<IBlobService, BlobService>();
        return services;
    }
}
