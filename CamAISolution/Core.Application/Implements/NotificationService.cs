using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class NotificationService(
    IBaseMapping mapping,
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    IAppLogging<NotificationService> logger,
    AccountNotificationSubject accountNotificationSubject
) : INotificationService
{
    public async Task<Notification> CreateNotification(CreateNotificationDto dto)
    {
        var notification = mapping.Map<CreateNotificationDto, Notification>(dto);
        try
        {
            await unitOfWork.BeginTransaction();
            notification = await unitOfWork.GetRepository<Notification>().AddAsync(notification);
            await unitOfWork.CompleteAsync();

            var sentToAccounts = (
                await unitOfWork.Accounts.GetAsync(expression: a => dto.SentToId.Contains(a.Id), disableTracking: false)
            ).Values;
            HashSet<AccountNotification> createdAccountNotifications = new();
            foreach (var acc in sentToAccounts)
            {
                createdAccountNotifications.Add(
                    await unitOfWork
                        .GetRepository<AccountNotification>()
                        .AddAsync(
                            new AccountNotification
                            {
                                AccountId = acc.Id,
                                NotificationId = notification.Id,
                                Status = NotificationStatus.Unread
                            }
                        )
                );
            }
            await unitOfWork.CompleteAsync();
            await unitOfWork.CommitTransaction();
            foreach (var createdAccountNotification in createdAccountNotifications)
                accountNotificationSubject.AccountNotification = createdAccountNotification;
            return notification;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            await unitOfWork.RollBack();
        }
        throw new ServiceUnavailableException("");
    }

    public async Task<PaginationResult<AccountNotification>> SearchNotification(SearchNotificationRequest req)
    {
        req.AccountId = jwtService.GetCurrentUser().Id;
        return await unitOfWork.GetRepository<AccountNotification>().GetAsync(new AccountNotificationSearchSpec(req));
    }

    public async Task<AccountNotification> UpdateStatus(Guid notificationId, NotificationStatus status)
    {
        var accountNotification = (
            await unitOfWork
                .GetRepository<AccountNotification>()
                .GetAsync(
                    expression: an =>
                        an.NotificationId == notificationId && an.AccountId == jwtService.GetCurrentUser().Id,
                    includeProperties: [nameof(AccountNotification.Notification)]
                )
        ).Values.First();
        if (accountNotification == null)
            throw new NotFoundException(
                typeof(Core.Domain.Entities.Notification),
                new { jwtService.GetCurrentUser().Id, notificationId }
            );
        if (accountNotification.Status != status)
        {
            accountNotification.Status = status;
            accountNotification = unitOfWork.GetRepository<AccountNotification>().Update(accountNotification);
            await unitOfWork.CompleteAsync();
        }
        return accountNotification;
    }
}
