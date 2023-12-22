using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class Province : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public virtual ICollection<District> Districts { get; set; } = new HashSet<District>();
}
