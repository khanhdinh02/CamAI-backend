namespace Core.Domain.Entities;

public class AccountNotification
{
    public Guid AccountId { get; set; }
    public Guid NotificationId { get; set; }
    public int StatusId { get; set; }
    public virtual NotificationStatus Status { get; set; } = null!;
    public virtual Account Account { get; set; } = null!;
    public virtual Notification Notification { get; set; } = null!;
}
