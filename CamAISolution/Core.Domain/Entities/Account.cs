using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Account : BusinessEntity
{
    public Account()
    {
        SentNotifications = new HashSet<Notification>();
        ReceivedNotifications = new HashSet<AccountNotification>();
    }

    public string? ExternalId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;
    public Gender? Gender { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public int? WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? BrandId { get; set; }
    public Role Role { get; set; }
    public AccountStatus AccountStatus { get; set; }

    public virtual Ward? Ward { get; set; }
    public virtual ICollection<Notification> SentNotifications { get; set; }

    /// <summary>
    /// The brand that this account (Brand Manager, Shop manager, Employee) is working for
    /// </summary>
    public virtual Brand? Brand { get; set; }

    [InverseProperty(nameof(Brand.BrandManager))]
    public virtual Brand? ManagingBrand { get; set; }
    public virtual Shop? ManagingShop { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<AccountNotification> ReceivedNotifications { get; set; }
}
