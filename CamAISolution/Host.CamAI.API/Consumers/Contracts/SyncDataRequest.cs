using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn(nameof(SyncDataRequest))]
public class SyncDataRequest
{
    public Guid EdgeBoxId { get; set; }
}
