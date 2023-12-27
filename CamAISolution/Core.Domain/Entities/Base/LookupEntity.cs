using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities.Base;

public abstract class LookupEntity : UpdatableEntity<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }

    [StringLength(20)]
    public virtual string Name { get; set; } = null!;
    public virtual string? Description { get; set; }
}
