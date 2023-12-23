using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class EdgeBoxActivity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EdgeBoxId { get; set; }
    public Guid OldStatusId { get; set; }
    public Guid NewStatusId { get; set; }
    public string? Description { get; set; }
    public Guid ModifiedById { get; set; }
    public DateTime ModifiedTime { get; set; }

    public virtual EdgeBox EdgeBox { get; set; } = null!;
    public virtual EdgeBoxStatus OldStatus { get; set; } = null!;
    public virtual EdgeBoxStatus NewStatus { get; set; } = null!;
    public virtual Account ModifiedBy { get; set; } = null!;
}
