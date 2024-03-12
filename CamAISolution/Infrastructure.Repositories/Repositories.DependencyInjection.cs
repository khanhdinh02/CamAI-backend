using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Base;
using Infrastructure.Repositories.Data;
using Infrastructure.Repositories.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public static class RepositoriesDependencyInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<CamAIContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped(typeof(IRepositorySpecificationEvaluator<>), typeof(RepositorySpecificationEvaluator<>));
        services.AddScoped(typeof(IRepository<>), typeof(Base.Repository<>));
        services.AddScoped<ICustomShopRepository, CustomShopRepository>();
        services.AddScoped<ICustomAccountRepository, CustomAccountRepository>();
        services.AddScoped<ICustomEmployeeRepository, CustomEmployeeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
