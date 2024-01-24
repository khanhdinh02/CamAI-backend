using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Account : BusinessEntity
{
    public Account()
    {
        SentNotifications = new HashSet<Notification>();
        Roles = new HashSet<Role>();
        ReceivedNotifications = new HashSet<AccountNotification>();
    }

    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public Gender? Gender { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public int? WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? WorkingShopId { get; set; }
    public int AccountStatusId { get; set; }

    /// <summary>
    /// Token which registered in Firebase Cloud Messaging is used for receiving notification
    /// </summary>
    public string? FCMToken { get; set; }

    public virtual Ward? Ward { get; set; }
    public virtual Shop? WorkingShop { get; set; }
    public virtual AccountStatus AccountStatus { get; set; } = null!;

    /// <summary>
    /// Notifications that sent by this account
    /// </summary>
    [InverseProperty(nameof(Notification.SentBy))]
    public virtual ICollection<Notification> SentNotifications { get; set; }

    /// <summary>
    /// The brand that this account (Brand Manager, Shop manager, Employee) is working for
    /// </summary>
    public virtual Brand? Brand { get; set; }

    [InverseProperty(nameof(Brand.BrandManager))]
    public virtual Brand? ManagingBrand { get; set; }

    [InverseProperty(nameof(Shop.ShopManager))]
    public virtual Shop? ManagingShop { get; set; }
    public virtual ICollection<Role> Roles { get; set; }
    public virtual ICollection<AccountNotification> ReceivedNotifications { get; set; }
}
