using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.DTO;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AccessTokenGuard]
public class NotificationsController(INotificationService notificationService, IBaseMapping mapping) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<NotificationDto>> CreateNotification(CreateNotificationDto dto) =>
        Ok(mapping.Map<Notification, NotificationDto>(await notificationService.CreateNotification(dto, true)));

    [HttpGet]
    public async Task<ActionResult<PaginationResult<Notification>>> SearchNotification(
        [FromQuery] SearchNotificationRequest req
    ) => Ok(mapping.Map<AccountNotification, NotificationDto>(await notificationService.SearchNotification(req)));

    [HttpPatch("{notificationId}/status/{statusId}")]
    public async Task<ActionResult<Notification>> UpdateNotificationStatus(Guid notificationId, int statusId)
    {
        return Ok(
            mapping.Map<AccountNotification, NotificationDto>(
                await notificationService.UpdateStatus(notificationId, statusId)
            )
        );
    }
}
