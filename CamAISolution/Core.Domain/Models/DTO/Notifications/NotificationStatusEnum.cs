using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class NotificationStatusEnum
{
    public const int Unread = 1;
    public const int Read = 2;
}
