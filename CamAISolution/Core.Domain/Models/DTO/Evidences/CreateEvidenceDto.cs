using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class CreateEvidenceDto
{
    public byte[] Content { get; set; } = null!;
    public EvidenceType EvidenceType { get; set; }
    public Camera Camera { get; set; }
}
