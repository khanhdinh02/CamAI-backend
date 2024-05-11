using Core.Application.Events.Args;
using Core.Domain.Events;

namespace Core.Application.Events;

public class BulkUpsertProgressSubject : ISubject<Domain.Events.IObserver<BulkUpsertCurrentProgressArgs>, BulkUpsertCurrentProgressArgs>
{
    private event EventHandler<BulkUpsertCurrentProgressArgs>? Created;
    public void Attach(Domain.Events.IObserver<BulkUpsertCurrentProgressArgs> observer)
    {
        Created += observer.Update;
    }

    public void Detach(Domain.Events.IObserver<BulkUpsertCurrentProgressArgs> observer)
    {
        Created -= observer.Update;
    }

    public void Notify(BulkUpsertCurrentProgressArgs e)
    {
        Created?.Invoke(this, e);
    }
}
