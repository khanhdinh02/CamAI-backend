using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface INotificationService
{
    Task<Notification> CreateNotification(CreateNotificationDto dto);
    Task<PaginationResult<AccountNotification>> SearchNotification(SearchNotificationRequest req);
    Task<AccountNotification> UpdateStatus(Guid notificationId, NotificationStatus status);
    Task UpdateAllNotificationToRead();
}
