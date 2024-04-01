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
    ICameraService cameraService
) : IConsumer<SyncDataRequestMessage>
{
    public async Task Consume(ConsumeContext<SyncDataRequestMessage> context)
    {
        var edgeBoxId = context.Message.EdgeBoxId;
        var ebInstall = (await edgeBoxInstallService.GetLatestInstallingByEdgeBox(edgeBoxId))!;
        var cameras = await cameraService.GetCameras(ebInstall.ShopId);

        syncObserver.SyncBrand(ebInstall.Shop.Brand, edgeBoxId.ToString("N"));
        syncObserver.SyncShop(ebInstall.Shop, edgeBoxId.ToString("N"));
        syncObserver.SyncCamera(cameras.Values, edgeBoxId.ToString("N"));
    }
}
