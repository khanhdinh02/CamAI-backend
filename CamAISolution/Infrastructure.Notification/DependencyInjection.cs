using Core.Application.Exceptions;
using Core.Domain.Interfaces.Services;
using Infrastructure.Notification.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Notification;

public static class DependencyInjection
{
    public static IServiceCollection AddNotification(this IServiceCollection services, GoogleSecret? secret)
    {
        if (secret == null)
            throw new ServiceUnavailableException("Cannot create service in backend");
        services.AddSingleton(secret);
        services.AddSingleton<FirebaseService>();
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }
}
