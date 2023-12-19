using Core.Application.Implements;
using Core.Domain.Interfaces.Services;

namespace Host.CamAI.API;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IBrandService, BrandService>();
        return services;
    }
}
