using System.Net.WebSockets;
using Core.Domain.Entities;
using Host.CamAI.API.Sockets;

namespace Host.CamAI.API.Middlewares;

public class NotificationSocket(RequestDelegate next, NotificationSocketManager notificationSocketManager)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (
            context.WebSockets.IsWebSocketRequest
            && context.Request.Path.HasValue
            && context.Request.Path.Value.Contains("notification")
        )
        {
            var account = (Account)context.Items[nameof(Account)]!;
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            notificationSocketManager.AddSocket(account.Id, socket);
            await ReadMessageAsync(socket, account.Id);
        }
        else
            await next(context);
    }

    private async Task ReadMessageAsync(WebSocket webSocket, Guid accountId)
    {
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer, 0, buffer.Length),
            CancellationToken.None
        );
        // wait until have close message
        while (result.MessageType != WebSocketMessageType.Close)
        {
            result = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer, 0, buffer.Length),
                CancellationToken.None
            );
        }
        notificationSocketManager.RemoveSocket(accountId);
    }
}
