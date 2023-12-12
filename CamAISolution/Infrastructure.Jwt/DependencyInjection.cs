namespace Infrastructure.Jwt;

using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection JwtDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = new JwtConfiguration
        {
            Audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(configuration)),
            Expired = int.Parse(configuration["Jwt:Expired"] ?? throw new ArgumentException("Not found expired")),
            Issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentException("Not found issuer"),
            JwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentException("Not found Jwt secret")
        };
        services.AddScoped<IJwtService>(p => new JwtService(jwtConfig));
        return services;
    }
}
