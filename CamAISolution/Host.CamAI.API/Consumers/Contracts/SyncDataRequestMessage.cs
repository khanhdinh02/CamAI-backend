using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("SyncDataRequest")]
public class SyncDataRequestMessage
{
    public Guid EdgeBoxId { get; set; }
}
