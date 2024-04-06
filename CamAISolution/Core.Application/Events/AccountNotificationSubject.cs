using Core.Application.Events.Args;
using Core.Domain.Entities;
using Core.Domain.Events;

namespace Core.Application.Events;

public class AccountNotificationSubject
    : ISubject<Domain.Events.IObserver<CreatedAccountNotificationArgs>, CreatedAccountNotificationArgs>
{
    public event EventHandler<CreatedAccountNotificationArgs> Created = null!;

    private AccountNotification accountNotification = null!;
    public AccountNotification AccountNotification
    {
        get { return accountNotification; }
        set
        {
            accountNotification = value;
            OnChange(new CreatedAccountNotificationArgs(value));
        }
    }

    public void OnChange(CreatedAccountNotificationArgs e)
    {
        Created.Invoke(this, e);
    }

    public void Attach(Domain.Events.IObserver<CreatedAccountNotificationArgs> observer)
    {
        Created += observer.Update;
    }

    public void Detach(Domain.Events.IObserver<CreatedAccountNotificationArgs> observer)
    {
        Created -= observer.Update;
    }
}
