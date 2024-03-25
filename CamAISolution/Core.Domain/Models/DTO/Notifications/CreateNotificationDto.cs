using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class CreateNotificationDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required]
    public string Content { get; set; } = null!;
    public NotificationPriority Priority { get; set; }
    public NotificationType Type { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public IEnumerable<Guid> SentToId { get; set; } = new HashSet<Guid>();
}
