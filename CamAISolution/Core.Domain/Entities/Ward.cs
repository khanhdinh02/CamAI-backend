using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Ward : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public Guid DistrictId { get; set; }

    public virtual District District { get; set; } = null!;
}
