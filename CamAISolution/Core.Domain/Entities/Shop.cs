using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Shop : BusinessEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? Phone { get; set; }
    public Guid WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? ShopManagerId { get; set; }
    public Guid BrandId { get; set; }
    public int ShopStatusId { get; set; }

    public virtual Account? ShopManager { get; set; }
    public virtual Ward Ward { get; set; } = null!;
    public virtual Brand Brand { get; set; } = null!;
    public virtual ShopStatus ShopStatus { get; set; } = null!;

    [InverseProperty(nameof(Account.WorkingShop))]
    public virtual ICollection<Account> Employees { get; set; } = new HashSet<Account>();
    public virtual ICollection<Camera> Cameras { get; set; } = new HashSet<Camera>();
    public virtual ICollection<EdgeBoxInstall> EdgeBoxInstalls { get; set; } = new HashSet<EdgeBoxInstall>();
}
