namespace Infrastructure.MessageQueue;

public class PublisherAttribute(string exchangeName) : MessageQueueEndpointAttribute(exchangeName)
{
    public override string QueueName => $"Publisher:{FormatTemplate(Template)}";
    public Uri Uri => new("exchange:" + QueueName);
}
