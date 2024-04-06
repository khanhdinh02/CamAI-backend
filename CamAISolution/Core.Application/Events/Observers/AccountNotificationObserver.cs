using Core.Application.Events.Args;
using Core.Domain.Entities;

namespace Core.Application.Events.Observers;

public class AccountNotificationObserver(Guid accountId) : Domain.Events.IObserver<CreatedAccountNotificationArgs>
{
    public AccountNotification? AccountNotification { get; private set; }

    public void Update(object? sender, CreatedAccountNotificationArgs args)
    {
        if (args.AccountNotification.AccountId == accountId)
            AccountNotification = args.AccountNotification;
    }
}
