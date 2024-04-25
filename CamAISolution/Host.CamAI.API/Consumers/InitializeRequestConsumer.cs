using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Utilities;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using Infrastructure.Observer;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}", ConsumerConstant.InitializeRequest)]
public class InitializeRequestConsumer(
    SyncObserver syncObserver,
    IEdgeBoxInstallService edgeBoxInstallService,
    IUnitOfWork unitOfWork,
    ICameraService cameraService,
    IAppLogging<InitializeRequestConsumer> logger
) : IConsumer<InitializeRequestMessage>
{
    public async Task Consume(ConsumeContext<InitializeRequestMessage> context)
    {
        var message = context.Message;
        var edgeBoxId = message.EdgeBoxId;
        logger.Info($"Receive sync request from edge box {edgeBoxId}");
        var ebInstall = (await edgeBoxInstallService.GetLatestInstallingByEdgeBox(edgeBoxId))!;
        var cameras = await cameraService.GetCamerasForEdgeBox(ebInstall.ShopId);

        syncObserver.SyncBrand(ebInstall.Shop.Brand, edgeBoxId.ToString("N"));
        syncObserver.SyncShop(ebInstall.Shop, edgeBoxId.ToString("N"));
        syncObserver.SyncCamera(cameras.Values, edgeBoxId.ToString("N"));
        syncObserver.SyncEdgeBox(ebInstall, edgeBoxId.ToString("N"));

        ebInstall.EdgeBoxInstallStatus = EdgeBoxInstallStatus.Working;
        ebInstall.IpAddress = message.IpAddress;
        ebInstall.OperatingSystem = message.OperatingSystem;
        ebInstall.EdgeBox.MacAddress = message.MacAddress;
        ebInstall.EdgeBox.Version = message.Version;
        ebInstall.LastSeen = DateTimeHelper.VNDateTime;
        ebInstall.NotificationSent = false;
        unitOfWork.EdgeBoxInstalls.Update(ebInstall);
        unitOfWork.EdgeBoxes.Update(ebInstall.EdgeBox);
        await unitOfWork.CompleteAsync();
    }
}
