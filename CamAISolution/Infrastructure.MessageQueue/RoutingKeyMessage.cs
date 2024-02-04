namespace Infrastructure.MessageQueue;

public abstract class RoutingKeyMessage
{
    public string RoutingKey { get; set; } = null!;
}
