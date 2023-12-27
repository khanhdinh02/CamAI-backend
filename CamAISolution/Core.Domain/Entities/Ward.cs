using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class Ward : BusinessEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public Guid DistrictId { get; set; }

    public virtual District District { get; set; } = null!;
}
