using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class AccountNotification
{
    public Guid AccountId { get; set; }
    public Guid NotificationId { get; set; }
    public NotificationStatus Status { get; set; }

    public virtual Account Account { get; set; } = null!;
    public virtual Notification Notification { get; set; } = null!;
}
