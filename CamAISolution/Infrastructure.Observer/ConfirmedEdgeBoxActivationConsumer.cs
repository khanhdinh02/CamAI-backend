using Core.Application.Specifications;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Services;
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
    IEdgeBoxService edgeBoxService,
    IUnitOfWork unitOfWork,
    INotificationService notificationService)
    : IConsumer<ConfirmedEdgeBoxActivationMessage>
{
    public async Task Consume(ConsumeContext<ConfirmedEdgeBoxActivationMessage> context)
    {
        logger.Info($"Confirmed activation message from edge box ID: {context.Message.EdgeBoxId}.");

        // Update edge box & edge box install status
        var edgeBoxInstall = (await edgeBoxInstallService.GetInstallingByEdgeBox(context.Message.EdgeBoxId))!;
        await edgeBoxInstallService.UpdateStatus(edgeBoxInstall.EdgeBoxId, EdgeBoxInstallStatus.Working);
        await edgeBoxService.UpdateStatus(context.Message.EdgeBoxId, EdgeBoxStatus.Active);

        // Send notification
        // TODO[Dat]: Discuss what to do if activation is failed
        var adminAccounts = unitOfWork.Accounts
            .GetAsync(new AccountByEmailSpec("admin").GetExpression(), takeAll: true)
            .GetAwaiter().GetResult().Values.Select(a => a.Id);
        await SendMessage(context.Message.IsActivatedSuccessfully, context.Message.EdgeBoxId, adminAccounts);
    }

    private Task SendMessage(bool isActivatedSuccessfully, Guid edgeBoxId, IEnumerable<Guid> sendToAccountIds)
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