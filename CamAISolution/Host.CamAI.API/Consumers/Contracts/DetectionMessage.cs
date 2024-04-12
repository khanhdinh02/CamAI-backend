using Core.Domain.Enums;
using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("ReceivedIncident")]
public class ReceivedIncidentMessage
{
    public Guid EdgeBoxId { get; set; }
    public Guid Id { get; set; }
    public int AiId { get; set; }
    public IncidentType IncidentType { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public virtual ICollection<ReceivedEvidence> Evidences { get; set; } = new HashSet<ReceivedEvidence>();
}

public class ReceivedEvidence
{
    public byte[] Content { get; set; }
    public EvidenceType EvidenceType { get; set; }
    public Guid CameraId { get; set; }
    public Guid EdgeBoxId { get; set; }
}
