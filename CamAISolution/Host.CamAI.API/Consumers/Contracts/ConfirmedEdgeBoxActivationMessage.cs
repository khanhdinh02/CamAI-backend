using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn(nameof(ConfirmedEdgeBoxActivationMessage))]
public class ConfirmedEdgeBoxActivationMessage
{
    public Guid EdgeBoxId { get; set; }
    public bool IsActivatedSuccessfully { get; set; }
}