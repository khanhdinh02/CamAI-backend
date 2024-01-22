using System.Runtime.InteropServices;
using System.Text;
using Infrastructure.MessageQueue.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Infrastructure.MessageQueue.Consumers;

public static class TestConsumer
{
    private static IConnection connection = null!;
    private static IModel channel = null!;

    public static async Task Run(RabbitMQConfiguration rabbitmqConfiguration, string exchange = "", string queue = "")
    {
        Initiate(rabbitmqConfiguration);
        await Consume(exchange, queue);
        channel.Close();
        connection.Close();
    }

    private static void Initiate(RabbitMQConfiguration rabbitmqConfiguration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = rabbitmqConfiguration.RabbitMQHost,
            Port = rabbitmqConfiguration.RabbitMQPort,
            UserName = rabbitmqConfiguration.RabbitMQUser,
            Password = rabbitmqConfiguration.RabbitMQPassword,
        };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
    }

    private static async Task Consume(string exchange = "", string queue = "")
    {
        bool isConnected = false;
        while (!isConnected)
        {
            try
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Direct);
                channel.QueueDeclare(queue, false, false, false, null);
                channel.QueueBind("", exchange, queue, null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, args) =>
                {
                    var message = Encoding.UTF8.GetString(args.Body.ToArray());
                    Console.WriteLine(message);
                    channel.BasicAck(args.DeliveryTag, false);
                };
                channel.BasicConsume(queue, false, consumer);
                isConnected = true;
            }
            catch (BrokerUnreachableException)
            {
                Console.WriteLine("Retrying connect to rabbitMQ");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
        Console.ReadLine();
    }
}
