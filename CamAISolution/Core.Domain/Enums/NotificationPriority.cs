using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum NotificationPriority
{
    Normal = 1,
    Warning = 2,
    Urgent = 3
}
