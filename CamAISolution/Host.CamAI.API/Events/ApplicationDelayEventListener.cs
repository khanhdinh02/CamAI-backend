using System.Collections.Concurrent;
using Core.Domain.Events;

namespace Host.CamAI.API.Events;

public class ApplicationDelayEventListener(IServiceProvider serviceProvider) : IApplicationDelayEventListener
{
    private readonly ConcurrentDictionary<string, IApplicationDelayEvent> events = new();
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> CancellationTokenSources = new();

    public Task AddEvent(string eventId, IApplicationDelayEvent appDelayEvent, bool isInvokedAfterAdded)
    {
        StopEvent(eventId);
        events[eventId] = appDelayEvent;
        return isInvokedAfterAdded ? InvokeEvent(eventId) : Task.CompletedTask;
    }

    public Task InvokeEvent(string eventId)
    {
        if (!events.TryGetValue(eventId, out var eventObj))
            return Task.CompletedTask;

        using var tokenSrc = new CancellationTokenSource();
        CancellationTokenSources.TryAdd(eventId, tokenSrc);
        return new TaskFactory().StartNew(
            () =>
            {
                //Delay the task
                return eventObj
                    .UseDelay()
                    // Do something after delay
                    .ContinueWith(async t =>
                    {
                        using var scope = serviceProvider.CreateScope();
                        // Set service instance in event class
                        foreach (var prop in eventObj.GetType().GetProperties())
                            prop.SetValue(eventObj, scope.ServiceProvider.GetRequiredService(prop.PropertyType), null);

                        events.TryRemove(eventId, out _);
                        // Trigger function
                        await eventObj.InvokeAsync();
                    });
            },
            tokenSrc.Token
        );
    }

    public Task StopEvent(string eventId)
    {
        if (CancellationTokenSources.TryGetValue(eventId, out var token))
        {
            events.TryRemove(eventId, out _);
            return token.CancelAsync();
        }
        return Task.CompletedTask;
    }
}
