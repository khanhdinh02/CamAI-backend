namespace Core.Domain.Entities.Base;

public abstract class BusinessEntity : UpdatableEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
}
