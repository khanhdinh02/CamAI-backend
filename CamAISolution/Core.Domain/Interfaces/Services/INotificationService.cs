using Core.Domain.Entities;
using Core.Domain.Models.DTO.Notifications;

namespace Core.Domain.Interfaces.Services;

public interface INotificationService
{
    /// <summary>
    /// Create notification
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="isSend">Whether Created Notification send to user</param>
    /// <returns></returns>
    Task<Notification> CreateNotification(CreateNotificationDto dto, bool isSend);
    Task<IEnumerable<Notification>> GetNotifications();
}
