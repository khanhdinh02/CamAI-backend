﻿using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Consumers;

namespace Host.CamAI.API.Controllers;

public class HumanCountWebSocket(WebSocket webSocket, IReportService reportService, Guid? shopId = default)
{
    private Task<WebSocketReceiveResult> receiveMessageTask = null!;

    public async Task Start()
    {
        var buffer =
            shopId == null
                ? await reportService.GetHumanCountStream()
                : await reportService.GetHumanCountStream(shopId.Value);

        receiveMessageTask = webSocket.ReceiveAsync(
            new ArraySegment<byte>(Array.Empty<byte>()),
            CancellationToken.None
        );
        while (webSocket.State == WebSocketState.Open)
        {
            Thread.Sleep(5000);
            if (await CheckCloseMessage())
                continue;

            var humanCount = new HumanCountModel
            {
                Time = DateTime.Now,
                Total = Random.Shared.Next(1, 5),
                ShopId = Guid.Empty
            };
            buffer.Write(humanCount);
            if (buffer.Count > 0)
            {
                var result = buffer.Read();
                await SendData(result);
            }
            // TODO [Duy]: check connection
        }
    }

    private async Task<bool> CheckCloseMessage()
    {
        if (!receiveMessageTask.IsCompleted)
            return false;

        var message = receiveMessageTask.Result;
        if (message.CloseStatus.HasValue)
        {
            await webSocket.CloseAsync(
                message.CloseStatus.Value,
                message.CloseStatusDescription,
                CancellationToken.None
            );
            return true;
        }

        receiveMessageTask = webSocket.ReceiveAsync(
            new ArraySegment<byte>(Array.Empty<byte>()),
            CancellationToken.None
        );
        return false;
    }

    private async Task SendData(HumanCountModel data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));

        await webSocket.SendAsync(
            new ArraySegment<byte>(dataBytes, 0, dataBytes.Length),
            WebSocketMessageType.Text,
            WebSocketMessageFlags.EndOfMessage,
            CancellationToken.None
        );
    }
}
