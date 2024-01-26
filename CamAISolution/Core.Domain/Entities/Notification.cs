using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Notification : BusinessEntity
{
    public Notification()
    {
        SentTo = new HashSet<AccountNotification>();
    }

    [Column(TypeName = "NVARCHAR(200)")]
    public string Title { get; set; } = null!;

    [Column(TypeName = "NVARCHAR(MAX)")]
    public string Content { get; set; } = null!;

    //TODO [Dat]: Clarify who sends notification
    public Guid SentById { get; set; }
    public int NotificationTypeId { get; set; }
    public virtual NotificationType NotificationType { get; set; } = null!;
    public virtual Account SentBy { get; set; } = null!;
    public virtual ICollection<AccountNotification> SentTo { get; set; }
}
