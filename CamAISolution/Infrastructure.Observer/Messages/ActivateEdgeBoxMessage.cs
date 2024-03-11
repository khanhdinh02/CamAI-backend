using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[Publisher(PublisherConstant.ActivateEdgeBox)]
[MessageUrn(nameof(ActivateEdgeBoxMessage))]
public class ActivateEdgeBoxMessage : RoutingKeyMessage;