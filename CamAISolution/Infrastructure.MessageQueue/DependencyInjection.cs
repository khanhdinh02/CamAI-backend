using Core.Application.Exceptions;
using Core.Domain.Interfaces.Services;
using Infrastructure.MessageQueue.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MessageQueue;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageQueue(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitmqConfig =
            configuration.GetRequiredSection("RabbitMQ").Get<RabbitMQConfiguration>()
            ?? throw new ServiceUnavailableException("Service is not ready");
        // services.AddMassTransit();
        // services.AddHostedService<MassTransitHostedService>();
        services.AddScoped(_ => rabbitmqConfig);
        services.AddScoped<IMessageQueueService, MessageQueueService>();
        return services;
    }
}
