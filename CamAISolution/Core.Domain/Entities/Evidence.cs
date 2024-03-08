using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Evidence : BusinessEntity
{
    public Uri? Uri { get; set; }
    public EvidenceType EvidenceType { get; set; }
    public Guid IncidentId { get; set; }
    public Guid CameraId { get; set; }
    public EvidenceStatus Status { get; set; }

    public virtual Incident Incident { get; set; } = null!;
    public virtual Camera Camera { get; set; } = null!;
}
