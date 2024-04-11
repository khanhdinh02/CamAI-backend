using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class CreateIncidentDto
{
    public Guid EdgeBoxId { get; set; }
    public Guid Id { get; set; }
    public int AiId { get; set; }
    public IncidentType IncidentType { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public virtual ICollection<CreateEvidenceDto> Evidences { get; set; } = new HashSet<CreateEvidenceDto>();
}
