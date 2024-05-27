using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class SupervisorAssignment : BaseEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public Guid? ShopId { get; set; }
    public Guid? SupervisorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public virtual Shop? Shop { get; set; }
    public virtual Account? Supervisor { get; set; }

    public virtual ICollection<Incident> Incidents { get; set; } = new HashSet<Incident>();
}
