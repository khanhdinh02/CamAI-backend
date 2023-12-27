using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Behavior : BusinessEntity
{
    public int BehaviorTypeId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CameraId { get; set; }
    public DateTime Time { get; set; }

    public virtual BehaviorType BehaviorType { get; set; } = null!;
    public virtual Account Account { get; set; } = null!;
    public virtual Camera Camera { get; set; } = null!;
    public virtual ICollection<Evidence> Evidences { get; set; } = new HashSet<Evidence>();
}
