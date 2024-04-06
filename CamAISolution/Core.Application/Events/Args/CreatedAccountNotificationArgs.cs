using Core.Domain.Entities;

namespace Core.Application.Events.Args;

public class CreatedAccountNotificationArgs(AccountNotification accountNotification) : EventArgs
{
    public AccountNotification AccountNotification => accountNotification;
}
