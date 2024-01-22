using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Notification : BusinessEntity
{
    public Notification()
    {
        SentTo = new HashSet<Account>();
    }

    [Column(TypeName = "NVARCHAR(200)")]
    public string Title { get; set; } = null!;

    [Column(TypeName = "NVARCHAR(MAX)")]
    public string Content { get; set; } = null!;
    public Guid SentById { get; set; }
    public int StatusId { get; set; }
    public virtual NotificationStatus Status { get; set; } = null!;
    public virtual Account SentBy { get; set; } = null!;

    public virtual ICollection<Account> SentTo { get; set; }
}
