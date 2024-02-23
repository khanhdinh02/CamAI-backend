using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Notification : BusinessEntity
{
    public Notification()
    {
        SentTo = new HashSet<AccountNotification>();
    }

    [StringLength(200)]
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    //TODO [Dat]: Clarify who sends notification
    public Guid SentById { get; set; }
    public NotificationType NotificationType { get; set; }

    public virtual Account SentBy { get; set; } = null!;
    public virtual ICollection<AccountNotification> SentTo { get; set; }
}
