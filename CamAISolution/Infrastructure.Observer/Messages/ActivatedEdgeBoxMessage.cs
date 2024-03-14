using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[Publisher(PublisherConstant.ActivateEdgeBox)]
[MessageUrn(nameof(ActivatedEdgeBoxMessage))]
public class ActivatedEdgeBoxMessage : RoutingKeyMessage
{
    public string Message { get; init; } = string.Empty;
}