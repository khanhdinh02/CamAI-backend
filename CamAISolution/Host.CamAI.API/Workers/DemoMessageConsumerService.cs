using System.Runtime.Intrinsics.Arm;
using System.Text;
using Infrastructure.MessageQueue.Models;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Host.CamAI.API.Workers;

public class DemoMessageConsumerService(IServiceProvider provider, ILogger<DemoMessageConsumerService> logger)
    : BackgroundService
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning("Message Comsumer was started");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning("Message Comsumer was stopped");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = provider.CreateScope();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                var config = scope
                    .ServiceProvider.GetRequiredService<IConfiguration>()
                    .GetSection("RabbitMQ")
                    .Get<RabbitMQConfiguration>();
                var connection = new ConnectionFactory
                {
                    Port = config!.RabbitMQPort,
                    HostName = config!.RabbitMQHost,
                    UserName = config!.RabbitMQUser,
                    Password = config!.RabbitMQPassword,
                };

                var channel = connection.CreateConnection().CreateModel();
                channel.ExchangeDeclare(exchange: "camai-exchange", type: ExchangeType.Direct);
                channel.QueueBind(queue: "demo", exchange: "camai-exchange", routingKey: "demo");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (ch, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    logger.LogInformation(body);
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume("demo", true, consumer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
