using System.Collections.Concurrent;
using System.Net.WebSockets;
using Core.Application.Events;
using Core.Application.Events.Args;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;

namespace Host.CamAI.API.Sockets;

public class NotificationSocketManager : Core.Domain.Events.IObserver<CreatedAccountNotificationArgs>
{
    // user id and websocket object
    private readonly ConcurrentDictionary<Guid, WebSocket> sockets = new();
    private readonly IServiceProvider serviceProvider;
    private readonly AccountNotificationSubject accountNotificationSubject;

    public NotificationSocketManager(
        IServiceProvider serviceProvider,
        AccountNotificationSubject accountNotificationSubject
    )
    {
        this.serviceProvider = serviceProvider;
        this.accountNotificationSubject = accountNotificationSubject;
        this.accountNotificationSubject.Attach(this);
    }

    public bool AddSocket(Guid accountId, WebSocket socket) => sockets.TryAdd(accountId, socket);

    public bool RemoveSocket(Guid accountId) => sockets.TryRemove(accountId, out _);

    public void Update(object? sender, CreatedAccountNotificationArgs args)
    {
        using var scope = serviceProvider.CreateScope();
        var mapper = scope.ServiceProvider.GetRequiredService<IBaseMapping>();
        foreach (var userId in args.SentToIds)
        {
            if (sockets.TryGetValue(userId, out var socket))
            {
                var jsonObj = System.Text.Json.JsonSerializer.Serialize(
                    mapper.Map<Notification, NotificationDto>(args.Notification)
                );
                var sentData = System.Text.Encoding.UTF8.GetBytes(jsonObj);
                socket.SendAsync(sentData, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    ~NotificationSocketManager()
    {
        accountNotificationSubject.Detach(this);
    }
}
