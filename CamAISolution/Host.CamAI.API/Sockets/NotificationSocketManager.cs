using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;

namespace Host.CamAI.API.Sockets;

public class NotificationSocketManager : Core.Domain.Events.IObserver<CreatedAccountNotificationArgs>
{
    // user id and user's websockets
    private readonly ConcurrentDictionary<Guid, HashSet<WebSocket>> sockets = new();
    private readonly IServiceProvider serviceProvider;
    private readonly AccountNotificationSubject accountNotificationSubject;
    private readonly IAppLogging<NotificationSocketManager> logger;

    private readonly JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public NotificationSocketManager(
        IServiceProvider serviceProvider,
        AccountNotificationSubject accountNotificationSubject,
        IAppLogging<NotificationSocketManager> logger
    )
    {
        this.serviceProvider = serviceProvider;
        this.accountNotificationSubject = accountNotificationSubject;
        this.logger = logger;
        this.accountNotificationSubject.Attach(this);
    }

    public bool AddSocket(Guid accountId, WebSocket socket)
    {
        logger.Info($"Add a new notification websocket");
        if (sockets.TryGetValue(accountId, out var userSockets))
        {
            return userSockets.Add(socket);
        }

        return sockets.TryAdd(accountId, new() { socket });
    }

    public bool RemoveSocket(Guid accountId, WebSocket socket)
    {
        logger.Info("Remove a notification websocket");
        if (!sockets.TryGetValue(accountId, out var userSockets))
            return false;

        if (!userSockets.Remove(socket))
            return false;

        logger.Info($"Close a websocket of {accountId}");
        socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);

        if (!userSockets.Any())
        {
            logger.Info(
                $"All websockets for notification of user {accountId} have been removed, trying to remove user {accountId}"
            );
            return sockets.TryRemove(accountId, out _);
        }
        return true;
    }

    public async void Update(object? sender, CreatedAccountNotificationArgs args)
    {
        logger.Info($"New notification has been notified");
        using var scope = serviceProvider.CreateScope();
        try
        {
            var mapper = scope.ServiceProvider.GetRequiredService<IBaseMapping>();
            foreach (var userId in args.SentToIds)
            {
                if (sockets.TryGetValue(userId, out var socket))
                {
                    var jsonObj = JsonSerializer.Serialize(
                        mapper.Map<Notification, NotificationDto>(args.Notification),
                        options
                    );
                    var sentData = System.Text.Encoding.UTF8.GetBytes(jsonObj);
                    if (sockets.TryGetValue(userId, out var userSockets))
                    {
                        foreach (var websocket in userSockets)
                        {
                            await websocket.SendAsync(
                                sentData,
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            );
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    ~NotificationSocketManager()
    {
        accountNotificationSubject.Detach(this);
    }
}
