namespace Infrastructure.MessageQueue;

[AttributeUsage(AttributeTargets.Class)]
public class MessageQueueEndpointAttribute(string queueName) : Attribute
{
    public string QueueName => queueName;
}
