using Core.Domain.Interfaces.Mappings;
using Infrastructure.Mapping.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mapping;

public static class DependencyInjection
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddScoped<IBaseMapping, BaseMapping>();
        return services;
    }
}
