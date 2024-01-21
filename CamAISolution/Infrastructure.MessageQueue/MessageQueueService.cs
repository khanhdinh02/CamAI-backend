using System.Text;
using Core.Domain.Interfaces.Services;
using Infrastructure.MessageQueue.Models;
using RabbitMQ.Client;

namespace Infrastructure.MessageQueue;

public class MessageQueueService : IMessageQueueService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public MessageQueueService(RabbitMQConfiguration mqConfig)
    {
        var factory = new ConnectionFactory
        {
            HostName = mqConfig.RabbitMQHost,
            UserName = mqConfig.RabbitMQUser,
            Password = mqConfig.RabbitMQPassword,
            Port = mqConfig.RabbitMQPort
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    ~MessageQueueService()
    {
        _connection.Close();
        _channel.Close();
    }

    public Task SendMessage<T>(T message, string sendToQueue, string exchange)
    {
        var jsonMessage = System.Text.Json.JsonSerializer.Serialize(message);
        var byteBody = Encoding.UTF8.GetBytes(jsonMessage);
        _channel.ExchangeDeclare(exchange, ExchangeType.Direct);
        _channel.QueueDeclare(sendToQueue, false, false, false, null);
        _channel.BasicPublish(exchange: exchange, routingKey: sendToQueue, basicProperties: null, body: byteBody);
        _channel.BasicQos(0, 1, true);
        return Task.CompletedTask;
    }
}
