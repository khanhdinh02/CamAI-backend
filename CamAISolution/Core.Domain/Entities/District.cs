using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class District : BusinessEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public Guid ProvinceId { get; set; }

    public virtual Province Province { get; set; } = null!;
    public virtual ICollection<Ward> Wards { get; set; } = new HashSet<Ward>();
}
