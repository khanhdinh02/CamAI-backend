using Core.Application.Events.Args;
using Core.Domain.Entities;
using Core.Domain.Events;

namespace Core.Application.Events;

public class AccountNotificationSubject
    : ISubject<Domain.Events.IObserver<CreatedAccountNotificationArgs>, CreatedAccountNotificationArgs>
{
    public event EventHandler<CreatedAccountNotificationArgs>? Created;

    public void Notify(CreatedAccountNotificationArgs e)
    {
        Created?.Invoke(this, e);
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
