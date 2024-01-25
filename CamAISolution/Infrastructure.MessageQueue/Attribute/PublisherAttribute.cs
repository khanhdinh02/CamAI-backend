namespace Infrastructure.MessageQueue;

public class PublisherAttribute(string exchangeName) : MessageQueueEndpointAttribute(exchangeName)
{
    public Uri Uri => new("exchange:" + QueueName);
}
