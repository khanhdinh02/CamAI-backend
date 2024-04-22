using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Brand : BusinessEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [ForeignKey(nameof(BrandManager))]
    public Guid? BrandManagerId { get; set; }
    public BrandStatus BrandStatus { get; set; }

    public string? Description { get; set; }

    [StringLength(100)]
    public string CompanyName { get; set; } = null!;

    [Url]
    public string? BrandWebsite { get; set; }

    [StringLength(200)]
    public string CompanyAddress { get; set; } = null!;
    public Guid CompanyWardId { get; set; }

    public Guid? LogoId { get; set; }
    public Guid? BannerId { get; set; }

    public virtual Image? Logo { get; set; }
    public virtual Image? Banner { get; set; }
    public virtual Account? BrandManager { get; set; }

    /// <summary>
    /// All people working for this brand, including brand managers, shop managers, employees
    /// </summary>
    [InverseProperty(nameof(Account.Brand))]
    public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    public virtual ICollection<Shop> Shops { get; set; } = new HashSet<Shop>();
    public virtual Ward Ward { get; set; } = null!;
}
