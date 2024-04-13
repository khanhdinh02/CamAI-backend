using Core.Domain.Enums;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[Publisher(PublisherConstant.UpdateData)]
[MessageUrn(nameof(EdgeBoxUpdateMessage))]
public class EdgeBoxUpdateMessage : RoutingKeyMessage
{
    public string? SerialNumber { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string Model { get; set; } = null!;
    public EdgeBoxActivationStatus ActivationStatus { get; set; }
}
