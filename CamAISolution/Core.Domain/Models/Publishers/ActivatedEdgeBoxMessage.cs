namespace Core.Domain.Models.Publishers;

public class ActivatedEdgeBoxMessage
{
    public string Message { get; init; } = string.Empty;
    public string RoutingKey { get; set; } = null!;
}