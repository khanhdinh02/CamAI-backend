using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public static class RepositoriesDependencyInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<DbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped(typeof(IRepository<>), typeof(Base.Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
