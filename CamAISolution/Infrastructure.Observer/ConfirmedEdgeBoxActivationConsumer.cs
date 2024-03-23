using Core.Application.Specifications;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Infrastructure.MessageQueue;
using Infrastructure.Observer.Messages;
using MassTransit;
using RabbitMQ.Client;

namespace Infrastructure.Observer;

[Consumer(ConsumerConstant.ConfirmedActivation, ConsumerConstant.ConfirmedActivation,
    exchangeType: ExchangeType.Fanout)]
public class ConfirmedEdgeBoxActivationConsumer(
    IAppLogging<ConfirmedEdgeBoxActivationConsumer> logger,
    IEdgeBoxInstallService edgeBoxInstallService,
    IUnitOfWork unitOfWork,
    INotificationService notificationService)
    : IConsumer<ConfirmedEdgeBoxActivationMessage>
{
    public async Task Consume(ConsumeContext<ConfirmedEdgeBoxActivationMessage> context)
    {
        logger.Info($"Confirmed activation message from edge box ID: {context.Message.EdgeBoxId}.");

        // Update edge box & edge box install status

        var edgeBoxInstall = (await edgeBoxInstallService.GetLatestInstallingByEdgeBox(context.Message.EdgeBoxId))!;
        edgeBoxInstall.ActivationStatus = EdgeBoxActivationStatus.Activated;
        edgeBoxInstall.EdgeBoxInstallStatus = EdgeBoxInstallStatus.Working;
        var edgeBox = (await unitOfWork.EdgeBoxes.GetByIdAsync(context.Message.EdgeBoxId))!;
        edgeBox.EdgeBoxStatus = EdgeBoxStatus.Active;
        try
        {
            unitOfWork.EdgeBoxInstalls.Update(edgeBoxInstall);
            unitOfWork.EdgeBoxes.Update(edgeBox);
            if (await unitOfWork.CompleteAsync() > 0)
            {
                // Send notification
                // TODO[Dat]: Discuss what to do if activation is failed
                var adminAccountIds =
                    (await unitOfWork.Accounts.GetAsync(new AccountByRoleSpec(Role.Admin).GetExpression(),
                        takeAll: true))
                    .Values.Select(a => a.Id);
                await SendNotification(context.Message.IsActivatedSuccessfully, context.Message.EdgeBoxId,
                    adminAccountIds);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    private Task SendNotification(bool isActivatedSuccessfully, Guid edgeBoxId, IEnumerable<Guid> sendToAccountIds)
    {
        var content = $"Edge box {edgeBoxId} is activated";
        var title = "Edge box is activated";
        var notificationType = NotificationType.Normal;
        if (!isActivatedSuccessfully)
        {
            content = $"Edge box {edgeBoxId} cannot be activated";
            title = "Edge box cannot be activated";
            notificationType = NotificationType.Urgent;
        }

        notificationService.CreateNotification(new CreateNotificationDto
        {
            Content = content,
            Title = title,
            NotificationType = notificationType,
            SentToId = sendToAccountIds
        }, true);
        return Task.CompletedTask;
    }
}