using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(INotificationService notificationService, IBaseMapping mapping) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
    {
        return Ok(
            (await notificationService.GetNotifications()).Select(n => mapping.Map<Notification, NotificationDto>(n))
        );
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> CreateNotification(CreateNotificationDto dto) =>
        Ok(mapping.Map<Notification, NotificationDto>(await notificationService.CreateNotification(dto, true)));
}
