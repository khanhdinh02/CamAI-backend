using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class BrandStatus : BaseEntity
{
    [StringLength(20)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<Brand> Brands { get; set; } = new HashSet<Brand>();
}
