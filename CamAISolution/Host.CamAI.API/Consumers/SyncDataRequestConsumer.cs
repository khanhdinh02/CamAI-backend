using Core.Domain;
using Core.Domain.Interfaces.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using Infrastructure.Observer;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}", ConsumerConstant.SyncDataRequest)]
public class SyncDataRequestConsumer(
    SyncObserver syncObserver,
    IEdgeBoxInstallService edgeBoxInstallService,
    ICameraService cameraService,
    IAppLogging<SyncDataRequestConsumer> logger
) : IConsumer<SyncDataRequestMessage>
{
    public async Task Consume(ConsumeContext<SyncDataRequestMessage> context)
    {
        var edgeBoxId = context.Message.EdgeBoxId;
        logger.Info($"Receive sync request from edge box {edgeBoxId}");
        var ebInstall = (await edgeBoxInstallService.GetLatestInstallingByEdgeBox(edgeBoxId))!;
        var cameras = await cameraService.GetCamerasForEdgeBox(ebInstall.ShopId);

        syncObserver.SyncBrand(ebInstall.Shop.Brand, edgeBoxId.ToString("N"));
        syncObserver.SyncShop(ebInstall.Shop, edgeBoxId.ToString("N"));
        syncObserver.SyncCamera(cameras.Values, edgeBoxId.ToString("N"));
    }
}
