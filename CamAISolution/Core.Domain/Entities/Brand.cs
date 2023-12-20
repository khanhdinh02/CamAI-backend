using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Brand : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public Guid? BrandManagerId { get; set; }
    public Guid BrandStatusId { get; set; }

    // TODO: add brand logo and banner

    public virtual Account? BrandManager { get; set; }
    public virtual BrandStatus BrandStatus { get; set; } = null!;
}
