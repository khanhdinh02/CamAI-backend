namespace Infrastructure.Jwt;

using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetSection("Jwt").Get<JwtConfiguration>();
        ArgumentNullException.ThrowIfNull(jwtConfiguration);

        services.AddScoped<IJwtService>(_ => new JwtService(jwtConfiguration));
        return services;
    }
}
