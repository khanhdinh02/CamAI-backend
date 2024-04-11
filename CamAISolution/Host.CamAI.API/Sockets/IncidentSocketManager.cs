using System.Collections.Concurrent;
using System.Dynamic;
using System.Net.WebSockets;
using System.Text.Json;
using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;

namespace Host.CamAI.API.Sockets;

public class IncidentSocketManager : Core.Domain.Events.IObserver<CreatedOrUpdatedIncidentArgs>
{
    // user id and websocket object
    private readonly ConcurrentDictionary<Guid, WebSocket> sockets = new();
    private readonly IncidentSubject incidentSubject;
    private readonly IServiceProvider serviceProvider;
    private readonly JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public IncidentSocketManager(IncidentSubject incidentSubject, IServiceProvider serviceProvider)
    {
        this.incidentSubject = incidentSubject;
        this.serviceProvider = serviceProvider;
        this.incidentSubject.Attach(this);
    }

    public bool AddSocket(Guid accountId, WebSocket socket) => sockets.TryAdd(accountId, socket);

    public bool RemoveSocket(Guid accountId) => sockets.TryRemove(accountId, out _);

    public async void Update(object? sender, CreatedOrUpdatedIncidentArgs args)
    {
        using var scope = serviceProvider.CreateScope();
        try
        {
            var mapper = scope.ServiceProvider.GetRequiredService<IBaseMapping>();
            dynamic messageToSend = new ExpandoObject();
            messageToSend.EventType = args.EventType;
            messageToSend.Incident = mapper.Map<Incident, IncidentDto>(args.Incident);
            var jsonObjStr = JsonSerializer.Serialize(messageToSend, options);
            var data = System.Text.Encoding.UTF8.GetBytes(jsonObjStr);
            if (sockets.TryGetValue(args.SentTo, out var socket))
            {
                await socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<IncidentSocketManager>>();
            logger.LogError(ex, ex.Message);
        }
    }

    ~IncidentSocketManager()
    {
        incidentSubject.Detach(this);
    }
}
