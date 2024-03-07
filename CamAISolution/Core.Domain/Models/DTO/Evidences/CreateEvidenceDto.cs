using Core.Domain.Enums;

namespace Core.Domain.Models.DTO.Evidences;

public class CreateEvidenceDto
{
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public EvidenceType EvidenceType { get; set; }
    public Guid CameraId { get; set; }
    public Guid EdgeBoxId { get; set; }
}
