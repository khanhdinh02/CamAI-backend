using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Brand : BusinessEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public Uri? LogoUri { get; set; }
    public Uri? BannerUri { get; set; }

    public Guid? BrandManagerId { get; set; }
    public int BrandStatusId { get; set; }

    public virtual Account? BrandManager { get; set; }
    public virtual BrandStatus BrandStatus { get; set; } = null!;
    public virtual ICollection<Shop> Shops { get; set; } = new HashSet<Shop>();
}
