using System.Net.WebSockets;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Host.CamAI.API.Sockets;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AccessTokenGuard]
public class NotificationsController(
    INotificationService notificationService,
    IBaseMapping mapping,
    NotificationSocketManager notificationSocketManager
) : ControllerBase
{
    // TODO[Dat]: remove this endpoint in production
    [HttpPost]
    public async Task<NotificationDto> CreateNotification(CreateNotificationDto dto) =>
        mapping.Map<Notification, NotificationDto>(await notificationService.CreateNotification(dto));

    [HttpGet]
    public async Task<PaginationResult<NotificationDto>> SearchNotification(
        [FromQuery] SearchNotificationRequest req
    ) => mapping.Map<AccountNotification, NotificationDto>(await notificationService.SearchNotification(req));

    /// <summary>
    /// Update status of notification
    /// </summary>
    /// <param name="notificationId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPatch("{notificationId}/status/{status}")]
    public async Task<ActionResult<NotificationDto>> UpdateNotificationStatus(
        Guid notificationId,
        NotificationStatus status
    )
    {
        return Ok(
            mapping.Map<AccountNotification, NotificationDto>(
                await notificationService.UpdateStatus(notificationId, status)
            )
        );
    }

    /// <summary>
    /// Get notification in real-time
    /// </summary>
    [HttpGet("new")]
    [AccessTokenGuard]
    public async Task GetNewestNotification()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest is false)
            return;
        var account = (Account)HttpContext.Items[nameof(Account)]!;
        var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        notificationSocketManager.AddSocket(account.Id, socket);
        await socket.SendAsync(
            System.Text.Encoding.UTF8.GetBytes("Connected"),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None
        );
        var buffer = new byte[1024 * 4];
        var result = await socket.ReceiveAsync(
            new ArraySegment<byte>(buffer, 0, buffer.Length),
            CancellationToken.None
        );

        // wait until have close message
        while (result.MessageType != WebSocketMessageType.Close)
        {
            result = await socket.ReceiveAsync(
                new ArraySegment<byte>(buffer, 0, buffer.Length),
                CancellationToken.None
            );
        }
        notificationSocketManager.RemoveSocket(account.Id);
    }
}
