namespace Core.Domain.Entities.Base;

public abstract class ActivityEntity : BaseEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public virtual string? Description { get; set; }
    public virtual Guid ModifiedById { get; set; }
    public virtual DateTime ModifiedTime { get; set; }

    public virtual Account ModifiedBy { get; set; } = null!;
}
