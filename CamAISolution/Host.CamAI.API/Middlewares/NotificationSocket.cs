using System.Net.WebSockets;
using Core.Application.Events;
using Core.Application.Events.Args;

namespace Host.CamAI.API.Middlewares;

public class NotificationSocket(RequestDelegate next) : Core.Domain.Events.IObserver<CreatedAccountNotificationArgs>
{
    private WebSocket webSocket = null!;

    public async Task InvokeAsync(HttpContext context)
    {
        if (
            context.WebSockets.IsWebSocketRequest
            && context.Request.Path.HasValue
            && context.Request.Path.Value.Contains("notification")
        )
        {
            using var scope = context.RequestServices.CreateScope();
            var accountNotificationSubject = scope.ServiceProvider.GetRequiredService<AccountNotificationSubject>();
            accountNotificationSubject.Attach(this);

            webSocket = await context.WebSockets.AcceptWebSocketAsync();
            while (webSocket.State == WebSocketState.Open)
            {
                // keep connection alive
            }
        }
        else
            await next(context);
    }

    // This method will triggered when notification is created
    public void Update(object? sender, CreatedAccountNotificationArgs args)
    {
        var jsonObject = System.Text.Json.JsonSerializer.Serialize(args.AccountNotification);
        var data = System.Text.Encoding.UTF8.GetBytes(jsonObject);
        webSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
