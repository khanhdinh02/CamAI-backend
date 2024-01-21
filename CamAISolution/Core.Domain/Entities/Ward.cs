using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Ward : BaseEntity<int>
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public int DistrictId { get; set; }

    public virtual District District { get; set; } = null!;
}
