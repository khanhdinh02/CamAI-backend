using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Jwt;
public static class JwtDependencyInjection
{
    public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetRequiredSection("Jwt"));
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}
