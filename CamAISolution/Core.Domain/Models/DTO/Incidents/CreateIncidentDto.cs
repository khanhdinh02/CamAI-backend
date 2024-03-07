using Core.Domain.Enums;
using Core.Domain.Models.DTO.Evidences;

namespace Core.Domain.DTO;

public class CreateIncidentDto
{
    public Guid EdgeBoxId { get; set; }
    public Guid Id { get; set; }
    public IncidentType IncidentType { get; set; }
    public DateTime Time { get; set; }
    public virtual ICollection<CreateEvidenceDto> Evidences { get; set; } = new HashSet<CreateEvidenceDto>();
}