using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Incident : BusinessEntity
{
    public int AiId { get; set; }
    public IncidentType IncidentType { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid EdgeBoxId { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.New;
    public Guid ShopId { get; set; }
    public Guid? EmployeeId { get; set; }

    public virtual EdgeBox EdgeBox { get; set; } = null!;
    public virtual Shop Shop { get; set; } = null!;

    public virtual Employee? Employee { get; set; }
    public virtual ICollection<Evidence> Evidences { get; set; } = new HashSet<Evidence>();
}
