using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class IncidentDto : BaseDto
{
    public IncidentType IncidentType { get; set; }
    public DateTime Time { get; set; }
    public Guid EdgeBoxId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? ShopId { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.New;

    public virtual ShopDto Shop { get; set; } = null!;
    public virtual EdgeBoxDto EdgeBox { get; set; } = null!;
    public virtual EmployeeDto? Employee { get; set; } = null!;
    public virtual ICollection<EvidenceDto> Evidences { get; set; } = new HashSet<EvidenceDto>();
}
