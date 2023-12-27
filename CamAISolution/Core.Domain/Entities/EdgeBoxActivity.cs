using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class EdgeBoxActivity : ActivityEntity
{
    public Guid EdgeBoxId { get; set; }
    public int? OldStatusId { get; set; }
    public int? NewStatusId { get; set; }

    public virtual EdgeBox EdgeBox { get; set; } = null!;
    public virtual EdgeBoxStatus? OldStatus { get; set; } = null!;
    public virtual EdgeBoxStatus? NewStatus { get; set; } = null!;
}
