using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<DbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        return services;
    }
}
