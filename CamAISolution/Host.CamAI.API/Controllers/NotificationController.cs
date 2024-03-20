using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AccessTokenGuard]
public class NotificationsController(INotificationService notificationService, IBaseMapping mapping) : ControllerBase
{
    [HttpPost]
    public async Task<NotificationDto> CreateNotification(CreateNotificationDto dto) =>
        mapping.Map<Notification, NotificationDto>(await notificationService.CreateNotification(dto, true));

    [HttpGet]
    public async Task<PaginationResult<NotificationDto>> SearchNotification(
        [FromQuery] SearchNotificationRequest req
    ) => mapping.Map<AccountNotification, NotificationDto>(await notificationService.SearchNotification(req));

    [HttpPatch("{notificationId}/status/{status}")]
    public async Task<NotificationDto> UpdateNotificationStatus(Guid notificationId, NotificationStatus status)
    {
        return mapping.Map<AccountNotification, NotificationDto>(
            await notificationService.UpdateStatus(notificationId, status)
        );
    }
}
