using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities;

public class Account : BaseEntity
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public Gender? Gender { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public Guid? WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? WorkingShopId { get; set; }
    public Guid AccountStatusId { get; set; }

    public virtual Ward? Ward { get; set; }
    public virtual Shop? WorkingShop { get; set; }
    public virtual AccountStatus AccountStatus { get; set; } = null!;
    public virtual Brand? Brand { get; set; }

    [InverseProperty(nameof(Shop.ShopManager))]
    public virtual Shop? ManagingShop { get; set; }
    public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();
}