using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO.Notifications;
using Core.Domain.Repositories;
using Core.Domain.Services;
using FirebaseAdmin.Messaging;

namespace Infrastructure.Notification;

public class NotificationService(
    IBaseMapping mapping,
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    IAppLogging<NotificationService> logger,
    FirebaseService firebaseService
) : INotificationService
{
    public async Task<IEnumerable<Core.Domain.Entities.Notification>> GetNotifications()
    {
        return (await unitOfWork.GetRepository<Core.Domain.Entities.Notification>().GetAsync(takeAll: true)).Values;
    }

    //TODO [Dat]: Validating who must be sent.
    public async Task<Core.Domain.Entities.Notification> CreateNotification(CreateNotificationDto dto, bool isSend)
    {
        var notification = mapping.Map<CreateNotificationDto, Core.Domain.Entities.Notification>(dto);
        notification.SentById = jwtService.GetCurrentUser().Id;
        notification.StatusId = NotificationStatusEnum.Unread;
        notification = await unitOfWork.GetRepository<Core.Domain.Entities.Notification>().AddAsync(notification);
        // await unitOfWork.CompleteAsync();
        if (isSend)
            logger.Info(await SendNotification(notification));
        return notification;
    }

    private Task<string> SendNotification(Core.Domain.Entities.Notification notification) =>
        firebaseService.Messaging.SendAsync(CreateMessage(notification.Title, notification.Content));

    private Message CreateMessage(string title, string body)
    {
        var message = new Message
        {
            Topic = "test",
            Notification = new FirebaseAdmin.Messaging.Notification { Body = body, Title = title, }
        };
        return message;
    }
}
