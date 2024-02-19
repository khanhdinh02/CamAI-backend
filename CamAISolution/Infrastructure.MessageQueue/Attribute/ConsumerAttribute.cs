using RabbitMQ.Client;

namespace Infrastructure.MessageQueue;

public class ConsumerAttribute(
    string queueName,
    string? exchangeName = null,
    string? routingKey = null,
    string exchangeType = ExchangeType.Fanout
) : MessageQueueEndpointAttribute(queueName)
{
    public override string QueueName => $"Consumer:{FormatTemplate(Template)}";
    public string RoutingKey => routingKey ?? QueueName;
    public string ExchangeName => $"Publisher:{FormatTemplate(exchangeName ?? Template)}";
    public string ExchangeType => exchangeType;
}
