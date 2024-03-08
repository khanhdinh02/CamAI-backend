using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class CreateEvidenceDto
{
    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public EvidenceType EvidenceType { get; set; }
    public Guid CameraId { get; set; }
    public Guid EdgeBoxId { get; set; }
}
