using System.Collections.Concurrent;
using System.Dynamic;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;

namespace Host.CamAI.API.Sockets;

public class IncidentSocketManager : Core.Domain.Events.IObserver<CreatedOrUpdatedIncidentArgs>
{
    // user id and user's websockets
    private readonly ConcurrentDictionary<Guid, HashSet<WebSocket>> sockets = new();
    private readonly IncidentSubject incidentSubject;
    private readonly IServiceProvider serviceProvider;
    private readonly JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() } };
    private readonly IAppLogging<IncidentSocketManager> logger;

    public IncidentSocketManager(IncidentSubject incidentSubject, IServiceProvider serviceProvider, IAppLogging<IncidentSocketManager> logger)
    {
        this.incidentSubject = incidentSubject;
        this.serviceProvider = serviceProvider;
        this.logger = logger;
        this.incidentSubject.Attach(this);
    }

    public bool AddSocket(Guid accountId, WebSocket socket)
    {
        logger.Info($"Add a new incident websocket");
        if (sockets.TryGetValue(accountId, out var userSockets))
        {
            return userSockets.Add(socket);
        }

        return sockets.TryAdd(accountId, new() { socket });
    }

    public bool RemoveSocket(Guid accountId, WebSocket socket)
    {
        logger.Info("Remove an incident websocket");
        if (!sockets.TryGetValue(accountId, out var userSockets))
            return false;

        if (!userSockets.Remove(socket))
            return false;
        if (userSockets.Any())
        {
            logger.Info($"All websockets for incident of user {accountId} have been removed, trying to remove user {accountId}");
            return sockets.TryRemove(accountId, out _);
        }
        return true;
    }

    public async void Update(object? sender, CreatedOrUpdatedIncidentArgs args)
    {
        logger.Info("New notification relate to incident has been notified");
        using var scope = serviceProvider.CreateScope();
        try
        {
            var mapper = scope.ServiceProvider.GetRequiredService<IBaseMapping>();
            dynamic messageToSend = new ExpandoObject();
            messageToSend.EventType = args.EventType.ToString();
            messageToSend.Incident = mapper.Map<Incident, IncidentDto>(args.Incident);
            var jsonObjStr = JsonSerializer.Serialize(messageToSend, options);
            var data = System.Text.Encoding.UTF8.GetBytes(jsonObjStr);
            if (sockets.TryGetValue(args.SentTo, out var userSockets))
            {
                foreach (var socket in userSockets)
                {
                    socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    ~IncidentSocketManager()
    {
        incidentSubject.Detach(this);
    }
}
