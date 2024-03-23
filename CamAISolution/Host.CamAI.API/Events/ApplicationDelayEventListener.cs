using System.Collections.Concurrent;
using Core.Domain.Events;

namespace Host.CamAI.API.Events;

public class ApplicationDelayEventListener(IServiceProvider serviceProvider) : IApplicationDelayEventListener
{
    private readonly ConcurrentDictionary<Guid, IApplicationDelayEvent> events = new();

    public Task AddEvent(Guid eventId, IApplicationDelayEvent appDelayEvent, bool isInvokedAfterAdded)
    {
        events[eventId] = appDelayEvent;
        if (isInvokedAfterAdded)
        {
            return InvokeEvent(eventId);
        }

        return Task.CompletedTask;
    }

    public Task InvokeEvent(Guid eventId)
    {
        if (!events.TryGetValue(eventId, out var eventObj))
        {
            return Task.CompletedTask;
        }

        return new TaskFactory().StartNew(() =>
        {
            //Delay the task
            return eventObj.UseDelay()
                // Do something after delay
                .ContinueWith(async t =>
                {
                    using var scope = serviceProvider.CreateScope();
                    // Set service instance in event class
                    foreach (var prop in eventObj.GetType().GetProperties())
                    {
                        prop.SetValue(eventObj, scope.ServiceProvider.GetRequiredService(prop.PropertyType), null);
                    }

                    events.TryRemove(eventId, out _);
                    // Trigger function
                    await eventObj.InvokeAsync();
                });
        });
    }
}