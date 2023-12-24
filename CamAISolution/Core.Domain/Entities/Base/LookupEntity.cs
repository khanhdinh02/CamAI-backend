using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities.Base;

public abstract class LookupEntity : UpdatableEntity<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }

    [StringLength(20)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
