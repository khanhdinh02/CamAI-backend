namespace Core.Domain.Events;

public interface IApplicationDelayEventListener
{
    Task AddEvent(string eventId, IApplicationDelayEvent appDelayEvent, bool isInvokedAfterAdded = false);
    Task InvokeEvent(string eventId);
    Task StopEvent(string eventId);
}
