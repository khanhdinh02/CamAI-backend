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
    public Guid SentById { get; set; }
    public virtual Account SentBy { get; set; } = null!;
    public virtual ICollection<AccountNotification> SentTo { get; set; }
}
