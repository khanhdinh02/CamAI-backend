using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities.Base;

public abstract class BusinessEntity : UpdatableEntity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
