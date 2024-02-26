using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum NotificationStatus
{
    Unread = 1,
    Read = 2
}
