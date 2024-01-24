using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [ForeignKey(nameof(BrandManager))]
    public Guid? BrandManagerId { get; set; }
    public int BrandStatusId { get; set; }

    public virtual Account? BrandManager { get; set; }
    public virtual BrandStatus BrandStatus { get; set; } = null!;

    /// <summary>
    /// All people working for this brand, including brand managers, shop managers, employees
    /// </summary>
    [InverseProperty(nameof(Account.Brand))]
    public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    public virtual ICollection<Shop> Shops { get; set; } = new HashSet<Shop>();
}
