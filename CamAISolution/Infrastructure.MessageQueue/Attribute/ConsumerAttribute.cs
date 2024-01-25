using RabbitMQ.Client;

namespace Infrastructure.MessageQueue;

public class ConsumerAttribute(
    string queueName,
    string? exchangeName = null,
    string? routingKey = null,
    string exchangeType = ExchangeType.Fanout
) : MessageQueueEndpointAttribute(queueName)
{
    public string RoutingKey => routingKey ?? QueueName;
    public string ExchangeName => exchangeName ?? QueueName;
    public string ExchangeType => exchangeType;
}
