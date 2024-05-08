using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class IncidentDto : BaseDto
{
    public int AiId { get; set; }
    public IncidentType IncidentType { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid EdgeBoxId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? ShopId { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.New;
    public Guid? InChargeAccountId { get; set; }
    public Guid? AssigningAccountId { get; set; }

    public ShopDto Shop { get; set; } = null!;
    public EdgeBoxDto EdgeBox { get; set; } = null!;
    public EmployeeDto? Employee { get; set; } = null!;
    public AccountDto? InChargeAccount { get; set; }
    public AccountDto? AssigningAccount { get; set; }
    public ICollection<EvidenceDto> Evidences { get; set; } = new HashSet<EvidenceDto>();
}
