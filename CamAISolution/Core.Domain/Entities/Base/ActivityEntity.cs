namespace Core.Domain.Entities.Base;

public abstract class ActivityEntity : BaseEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public Guid? ModifiedById { get; set; }
    public DateTime ModifiedTime { get; set; }

    public virtual Account? ModifiedBy { get; set; } = null!;
}
