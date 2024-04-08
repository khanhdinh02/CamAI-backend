using Core.Domain.Entities;

namespace Core.Application.Events.Args;

public class CreatedAccountNotificationArgs(Notification notification, IEnumerable<Guid> sentToIds) : EventArgs
{
    public IEnumerable<Guid> SentToIds => sentToIds;
    public Notification Notification => notification;
}
