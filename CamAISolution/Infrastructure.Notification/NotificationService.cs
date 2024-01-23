using Core.Application;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.DTO;
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
    public async Task<Core.Domain.Entities.Notification> CreateNotification(CreateNotificationDto dto, bool isSend)
    {
        var notification = mapping.Map<CreateNotificationDto, Core.Domain.Entities.Notification>(dto);
        notification.SentById = jwtService.GetCurrentUser().Id;
        try
        {
            await unitOfWork.BeginTransaction();
            notification = await unitOfWork.GetRepository<Core.Domain.Entities.Notification>().AddAsync(notification);
            await unitOfWork.CompleteAsync();
            var sentToAccounts = (
                await unitOfWork.Accounts.GetAsync(expression: a => dto.SentToId.Contains(a.Id), disableTracking: false)
            ).Values;
            foreach (var acc in sentToAccounts)
            {
                await unitOfWork
                    .GetRepository<AccountNotification>()
                    .AddAsync(
                        new AccountNotification
                        {
                            AccountId = acc.Id,
                            NotificationId = notification.Id,
                            StatusId = NotificationStatusEnum.Unread
                        }
                    );
            }
            await unitOfWork.CompleteAsync();
            await unitOfWork.CommitTransaction();
            if (isSend)
                foreach (var acc in sentToAccounts.Where(a => !string.IsNullOrEmpty(a.FCMToken)))
                    await firebaseService.Messaging.SendAsync(
                        CreateMessage(notification.Title, notification.Content, token: acc.FCMToken)
                    );
            return notification;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            await unitOfWork.RollBack();
        }
        throw new ServiceUnavailableException("Cannot do action");
    }

    private Message CreateMessage(
        string title,
        string body,
        Dictionary<string, string>? data = null,
        string? token = null,
        string? topic = null
    )
    {
        var message = new Message
        {
            Notification = new FirebaseAdmin.Messaging.Notification { Body = body, Title = title, },
            Data = data,
        };
        if (!string.IsNullOrEmpty(topic))
            message.Topic = topic;
        if (!string.IsNullOrEmpty(token))
            message.Token = token;
        return message;
    }

    public async Task<PaginationResult<AccountNotification>> SearchNotification(SearchNotificationRequest req)
    {
        req.AccountId = jwtService.GetCurrentUser().Id;
        return await unitOfWork.GetRepository<AccountNotification>().GetAsync(new AccountNotificationSearchSpec(req));
    }

    public async Task<AccountNotification> UpdateStatus(Guid notificationId, int statusId)
    {
        var accountNotification = await unitOfWork
            .GetRepository<AccountNotification>()
            .GetByIdAsync(jwtService.GetCurrentUser().Id, notificationId);
        if (accountNotification == null)
            throw new NotFoundException(
                typeof(Core.Domain.Entities.Notification),
                new { jwtService.GetCurrentUser().Id, notificationId }
            );
        accountNotification.StatusId = statusId;
        accountNotification = unitOfWork.GetRepository<AccountNotification>().Update(accountNotification);
        await unitOfWork.CompleteAsync();
        return accountNotification;
    }
}
