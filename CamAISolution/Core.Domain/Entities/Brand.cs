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

    [ForeignKey(nameof(BrandManager))]
    public Guid? BrandManagerId { get; set; }
    public int BrandStatusId { get; set; }

    public Guid? LogoId { get; set; }
    public Guid? BannerId { get; set; }

    public virtual Image? Logo { get; set; }
    public virtual Image? Banner { get; set; }
    public virtual Account? BrandManager { get; set; }
    public virtual BrandStatus BrandStatus { get; set; } = null!;

    /// <summary>
    /// All people working for this brand, including brand managers, shop managers, employees
    /// </summary>
    [InverseProperty(nameof(Account.Brand))]
    public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    public virtual ICollection<Shop> Shops { get; set; } = new HashSet<Shop>();
}
