namespace Infrastructure.MessageQueue;

public class PublisherAttribute(string exchangeName) : MessageQueueEndpointAttribute(exchangeName)
{
    public override string QueueName => $"Publisher:{QName}";
    public Uri Uri => new("exchange:" + QueueName);
}
