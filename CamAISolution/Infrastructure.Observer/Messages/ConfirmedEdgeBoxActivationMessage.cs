using MassTransit;

namespace Infrastructure.Observer.Messages;

[MessageUrn(nameof(ConfirmedEdgeBoxActivationMessage))]
public class ConfirmedEdgeBoxActivationMessage
{
    public Guid EdgeBoxId { get; set; }
    public bool IsActivatedSuccessfully { get; set; }
}