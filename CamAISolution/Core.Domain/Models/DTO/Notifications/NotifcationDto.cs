using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class NotificationDto : BaseDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public NotificationPriority Priority { get; set; }
    public NotificationType Type { get; set; }
    public string EntityName { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public NotificationStatus Status { get; set; }
}
