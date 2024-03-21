namespace Core.Domain.Events;

public interface IApplicationDelayEventListener
{
    Task AddEvent(Guid eventId, IApplicationDelayEvent appDelayEvent, bool isInvokedAfterAdded = false);
    Task InvokeEvent(Guid eventId);
}