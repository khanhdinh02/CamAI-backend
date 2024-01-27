using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Behavior : BusinessEntity
{
    public int BehaviorTypeId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? CameraId { get; set; }
    public DateTime Time { get; set; }

    public virtual BehaviorType BehaviorType { get; set; } = null!;
    public virtual Employee? Employee { get; set; }
    public virtual Camera? Camera { get; set; }
    public virtual ICollection<Evidence> Evidences { get; set; } = new HashSet<Evidence>();
}
