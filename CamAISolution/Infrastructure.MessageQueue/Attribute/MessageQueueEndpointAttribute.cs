namespace Infrastructure.MessageQueue;

[AttributeUsage(AttributeTargets.Class)]
public abstract class MessageQueueEndpointAttribute(string queueName) : Attribute
{
    protected readonly string Template = queueName;
    public abstract string QueueName { get; }

    protected static string FormatTemplate(string template)
    {
        var str = template.Replace("{MachineName}", Environment.MachineName);
        return str;
    }
}
