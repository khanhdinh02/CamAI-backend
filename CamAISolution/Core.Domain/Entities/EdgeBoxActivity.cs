using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class EdgeBoxActivity : ActivityEntity
{
    public Guid EdgeBoxId { get; set; }
    public EdgeBoxStatus? OldStatus { get; set; } = null!;
    public EdgeBoxStatus? NewStatus { get; set; } = null!;

    public virtual EdgeBox EdgeBox { get; set; } = null!;
}
