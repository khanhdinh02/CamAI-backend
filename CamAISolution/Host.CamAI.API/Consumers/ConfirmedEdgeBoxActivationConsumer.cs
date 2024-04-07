using Core.Domain;
using Core.Domain.Constants;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Events;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;
using RabbitMQ.Client;

namespace Host.CamAI.API.Consumers;

[Consumer(
    ConsumerConstant.ConfirmedActivation,
    ConsumerConstant.ConfirmedActivation,
    exchangeType: ExchangeType.Fanout
)]
public class ConfirmedEdgeBoxActivationConsumer(
    IAppLogging<ConfirmedEdgeBoxActivationConsumer> logger,
    IEdgeBoxInstallService edgeBoxInstallService,
    IApplicationDelayEventListener applicationDelayEventListener,
    IUnitOfWork unitOfWork,
    INotificationService notificationService,
    ICacheService cacheService
) : IConsumer<ConfirmedEdgeBoxActivationMessage>
{
    public async Task Consume(ConsumeContext<ConfirmedEdgeBoxActivationMessage> context)
    {
        logger.Info($"Confirmed activation message from edge box ID: {context.Message.EdgeBoxId}.");

        try
        {
            var edgeBoxInstall = (await edgeBoxInstallService.GetLatestInstallingByEdgeBox(context.Message.EdgeBoxId))!;
            if (edgeBoxInstall.ActivationStatus == EdgeBoxActivationStatus.Activated)
            {
                logger.Info($"Edge box id {context.Message.EdgeBoxId} is already activated");
                return;
            }

            if (context.Message.IsActivatedSuccessfully is false)
            {
                logger.Info($"Edge box id {context.Message.EdgeBoxId} activated failed");
                await edgeBoxInstallService.UpdateActivationStatus(
                    edgeBoxInstall.Id,
                    EdgeBoxActivationStatus.Failed,
                    "Edge box activation failed"
                );
            }
            else
            {
                logger.Info($"Edge box id {context.Message.EdgeBoxId} activated successfully, stopping delay event");
                await edgeBoxInstallService.UpdateActivationStatus(
                    edgeBoxInstall.Id,
                    EdgeBoxActivationStatus.Activated,
                    "Edge box activated"
                );
                await applicationDelayEventListener.StopEvent($"ActivateEdgeBox{edgeBoxInstall.Id:N}");
            }
            await applicationDelayEventListener.StopEvent($"ActivateEdgeBox{edgeBoxInstall.Id:N}");

            if (await unitOfWork.CompleteAsync() > 0)
            {
                var brandManagerAccountIds = (
                    await unitOfWork.Accounts.GetAsync(
                        a =>
                            a.Role == Role.BrandManager
                            && a.ManagingShop != null
                            && a.ManagingShop.Id == edgeBoxInstall.ShopId,
                        takeAll: true
                    )
                )
                    .Values.Select(a => a.Id)
                    .ToList();

                await SendNotification(
                    context.Message.IsActivatedSuccessfully,
                    context.Message.EdgeBoxId,
                    brandManagerAccountIds
                );
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    private Task SendNotification(bool isActivatedSuccessfully, Guid edgeBoxId, IList<Guid> sendToAccountIds)
    {
        var content = $"Edge box {edgeBoxId} is activated";
        var title = "Edge box is activated";
        var notificationType = NotificationType.EdgeBoxInstallActivation;
        var priority = NotificationPriority.Normal;
        if (!isActivatedSuccessfully)
        {
            var adminId = cacheService.Get<Account>(ApplicationCacheKey.Admin)!.Id;
            sendToAccountIds.Add(adminId);
            content = $"Edge box {edgeBoxId} cannot be activated";
            title = "Edge box cannot be activated";
            priority = NotificationPriority.Urgent;
        }
        logger.Info($"Send notification to admin and manager {string.Join(", ", sendToAccountIds)}");

        notificationService.CreateNotification(
            new CreateNotificationDto
            {
                Content = content,
                Title = title,
                Type = notificationType,
                Priority = priority,
                SentToId = sendToAccountIds
            }
        );
        return Task.CompletedTask;
    }
}
