using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class District : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public Guid ProvinceId { get; set; }

    public virtual Province Province { get; set; } = null!;
    public virtual ICollection<Ward> Wards { get; set; } = new HashSet<Ward>();
}
