using Core.Application.Implements;
using Core.Domain.Interfaces.Services;

namespace Host.CamAI.API;

public static class DependencyInjection
{
    public static IServiceCollection ApiDenpendencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>()
        return services;
    }
}
