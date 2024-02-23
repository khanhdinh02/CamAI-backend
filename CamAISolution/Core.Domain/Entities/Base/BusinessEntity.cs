using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities.Base;

public abstract class BusinessEntity : BaseEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime ModifiedDate { get; set; }

    [Timestamp]
    public virtual byte[]? Timestamp { get; set; }
}
