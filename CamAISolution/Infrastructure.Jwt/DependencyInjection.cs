using Core.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Jwt
{
    public static class DependencyInjection
    {
        public static IServiceCollection JwtDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
