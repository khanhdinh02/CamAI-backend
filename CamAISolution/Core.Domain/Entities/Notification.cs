using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Notification : BusinessEntity
{
    [StringLength(200)]
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public NotificationPriority Priority { get; set; }
    public NotificationType Type { get; set; }
    public Guid? RelatedEntityId { get; set; }

    public virtual ICollection<AccountNotification> SentTo { get; set; } = new HashSet<AccountNotification>();
}
