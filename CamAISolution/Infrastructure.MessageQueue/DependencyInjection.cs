using System.Net;
using Core.Application.Exceptions;
using Core.Domain.Interfaces.Services;
using Infrastructure.MessageQueue.Consumers;
using Infrastructure.MessageQueue.Models;
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
        services.AddSingleton(_ => rabbitmqConfig);
        services.AddScoped<IMessageQueueService, MessageQueueService>();
        Task.Run(() => TestConsumer.Run(rabbitmqConfig, "camai-exchange", "demo"));
        return services;
    }
}
