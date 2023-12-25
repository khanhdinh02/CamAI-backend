using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Shop : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? Phone { get; set; }
    public Guid WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? ShopManagerId { get; set; }
    public Guid ShopStatusId { get; set; }

    public virtual Account? ShopManager { get; set; } = null!;
    public virtual Ward Ward { get; set; } = null!;
    public virtual ShopStatus ShopStatus { get; set; } = null!;

    [InverseProperty(nameof(Account.WorkingShop))]
    public virtual ICollection<Account> Employees { get; set; } = new HashSet<Account>();
}