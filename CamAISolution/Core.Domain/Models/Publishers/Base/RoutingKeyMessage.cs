namespace Core.Domain.Models.Publishers.Base;

public abstract class RoutingKeyMessage
{
    public string RoutingKey { get; set; } = null!;
}
