using Core.Domain.Models.Publishers.Base;

namespace Core.Domain.Models.Publishers;

public class ActivatedEdgeBoxMessage : RoutingKeyMessage
{
    public string Message { get; init; } = string.Empty;
}
