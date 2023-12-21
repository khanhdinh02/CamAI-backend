using Core.Application;
using Core.Domain.Services;

namespace Host.CamAI.API;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IShopService, ShopService>();
        return services;
    }
}
