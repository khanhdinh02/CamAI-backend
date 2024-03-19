using Core.Domain.Interfaces.Emails;
using Core.Domain.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailConfiguration>(configuration.GetRequiredSection("Email"));
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
