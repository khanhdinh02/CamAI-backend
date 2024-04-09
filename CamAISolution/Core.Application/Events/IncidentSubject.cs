using Core.Application.Events.Args;
using Core.Domain.Events;

namespace Core.Application.Events;

public class IncidentSubject
    : ISubject<Domain.Events.IObserver<CreatedOrUpdatedIncidentArgs>, CreatedOrUpdatedIncidentArgs>
{
    private event EventHandler<CreatedOrUpdatedIncidentArgs>? CreatedOrUpdated;

    public void Notify(CreatedOrUpdatedIncidentArgs e)
    {
        CreatedOrUpdated?.Invoke(this, e);
    }

    public void Attach(Domain.Events.IObserver<CreatedOrUpdatedIncidentArgs> observer)
    {
        CreatedOrUpdated += observer.Update;
    }

    public void Detach(Domain.Events.IObserver<CreatedOrUpdatedIncidentArgs> observer)
    {
        CreatedOrUpdated -= observer.Update;
    }
}
