using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Evidence : BusinessEntity
{
    public Uri? Uri { get; set; }
    public int EvidenceTypeId { get; set; }

    public virtual EvidenceType EvidenceType { get; set; } = null!;
}
