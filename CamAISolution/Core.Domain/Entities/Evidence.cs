using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Evidence : BusinessEntity
{
    public Uri? Uri { get; set; }
    public int EvidenceTypeId { get; set; }
    public EvidenceType EvidenceType { get; set; }
}
