using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models;
using Core.Domain.Models.DTO;

namespace Core.Domain.Interfaces.Services;

public interface INotificationService
{
    /// <summary>
    /// Create notification
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="willSend">Whether Created Notification send to user (Push notification to FCM)</param>
    /// <returns></returns>
    Task<Notification> CreateNotification(CreateNotificationDto dto, bool willSend);
    Task<PaginationResult<AccountNotification>> SearchNotification(SearchNotificationRequest req);
    Task<AccountNotification> UpdateStatus(Guid notificationId, NotificationStatus status);
}
