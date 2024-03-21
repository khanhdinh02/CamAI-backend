using Core.Application.Exceptions;
using Core.Application.Specifications;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Events;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;

namespace Core.Application.Events;

public class FirstCheckEdgeBoxAfterActivationDelayEvent(TimeSpan delay, Guid edgeBoxId, Guid edgeBoxInstallId)
    : IApplicationDelayEvent
{
    public IUnitOfWork UnitOfWork { get; set; } = null!;
    public INotificationService NotificationService { get; set; } = null!;
    public IAppLogging<FirstCheckEdgeBoxAfterActivationDelayEvent> Logger { get; set; } = null!;

    public Task UseDelay()
    {
        return Task.Delay(delay);
    }

    public async Task InvokeAsync()
    {
        try
        {
            var edgeBoxInstall = await UnitOfWork.EdgeBoxInstalls.GetByIdAsync(edgeBoxInstallId) ??
                                 throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);
            var edgeBox = await UnitOfWork.EdgeBoxes.GetByIdAsync(edgeBoxId) ??
                          throw new NotFoundException(typeof(EdgeBox), edgeBoxId);
            var sentToAdmin =
                (await UnitOfWork.Accounts.GetAsync(new AccountByEmailSpec("admin").GetExpression(), takeAll: true))
                .Values
                .Select(a => a.Id);

            // Edge box still disconnected from the server
            if (edgeBox.EdgeBoxStatus != EdgeBoxStatus.Active)
            {
                await NotificationService.CreateNotification(new CreateNotificationDto
                {
                    Content = $"Edge Box {edgeBoxId} is still disconnected from server",
                    NotificationType = NotificationType.Urgent,
                    Title = "Edge Box is disconnected from server",
                    SentToId = sentToAdmin
                }, true);
                return;
            }

            // Edge box is activated but edge box install isn't working
            if (edgeBoxInstall.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Working)
            {
                await NotificationService.CreateNotification(new CreateNotificationDto
                {
                    Content = $"Edge box is activated but edge box install-{edgeBoxInstallId} is not working",
                    Title = "Edge box install is not working",
                    NotificationType = NotificationType.Urgent,
                    SentToId = sentToAdmin
                }, true);
                return;
            }

            await NotificationService.CreateNotification(new CreateNotificationDto
            {
                Content = $"Edge box-{edgeBoxId} is successfully activated",
                Title = "Edge box is activated",
                NotificationType = NotificationType.Normal,
                SentToId = sentToAdmin
            }, true);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message, ex);
        }
    }
}