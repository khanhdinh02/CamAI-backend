using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class IncidentDto
{
    public IncidentType IncidentType { get; set; }
    public DateTime Time { get; set; }
    public Guid EdgeBoxId { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.New;

    public virtual EdgeBoxDto EdgeBox { get; set; } = null!;
    public virtual IEnumerable<EmployeeDto> Employees { get; set; } = new HashSet<EmployeeDto>();
    public virtual ICollection<EvidenceDto> Evidences { get; set; } = new HashSet<EvidenceDto>();
}
