using Core.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Notification;
public static class DependencyInjection
{
    public static IServiceCollection AddNotification(this IServiceCollection services)
    {
        services.AddSingleton<FirebaseService>();
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }
}
