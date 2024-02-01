using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class NotificationTypeEnum
{
    public const int Normal = 1;
    public const int Warnning = 2;
    public const int Urgent = 3;
}
