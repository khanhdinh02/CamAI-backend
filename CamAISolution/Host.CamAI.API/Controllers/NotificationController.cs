using Core.Domain.DTO;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationController(INotificationService notificationService, IBaseMapping mapping) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<NotifcationDto>> GetNotifications() =>  Ok(await notificationService.GetNotifications());

    [HttpPost]
    public async Task<ActionResult<NotifcationDto>> CreateNotification(CreateNotificationDto dto) => Ok(await notificationService.CreateNotification(dto, true));
}
