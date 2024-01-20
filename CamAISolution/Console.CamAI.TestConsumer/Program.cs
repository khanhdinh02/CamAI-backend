using System.Text;
using Infrastructure.MessageQueue.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Console.CamAI.TestConsumer;

public static class Program
{
    private static Task Main(string[] args)
    {
        var appsettingName = "appsettings.Development.json";
        if (args.Length > 0 && (args.Contains("--host") || args.Contains("-h")))
            appsettingName = "appsettings.json";
        bool isConnected = false;
        while (!isConnected)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(
                        $@"{Directory.GetParent(Directory.GetCurrentDirectory())}\Host.CamAI.API\{appsettingName}",
                        true,
                        true
                    )
                    .Build();
                var rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>()!;
                var factory = new ConnectionFactory
                {
                    Port = rabbitMQConfig.RabbitMQPort,
                    HostName = rabbitMQConfig.RabbitMQHost,
                    UserName = rabbitMQConfig.RabbitMQUser,
                    Password = rabbitMQConfig.RabbitMQPassword,
                };
                IConnection connection;
                connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "camai-exchange", type: ExchangeType.Direct);
                channel.QueueBind(queue: "demo", exchange: "camai-exchange", routingKey: "demo");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (ch, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    System.Console.WriteLine(body);
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume("demo", false, consumer);
                isConnected = true;
            }
            catch (BrokerUnreachableException)
            {
                System.Console.WriteLine("Retrying connect to RabbitMQ");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
        System.Console.ReadLine();
        return Task.CompletedTask;
    }
}
