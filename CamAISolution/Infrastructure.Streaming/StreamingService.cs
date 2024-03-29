using Core.Application.Exceptions;
using Core.Domain.Interfaces.Services;
using MassTransit;

namespace Infrastructure.Streaming;

public class StreamingService(
    ICameraService cameraService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IPublishEndpoint bus
) : IStreamingService
{
    public async Task<Uri> StreamCamera(Guid id)
    {
        var camera = await cameraService.GetCameraById(id);
        var ebInstall =
            await edgeBoxInstallService.GetCurrentInstallationByShop(camera.ShopId)
            ?? throw new BadRequestException("There is not edge box installed in shop");

        var relayPort = WebsocketRelayProcessManager.Run(camera.Id.ToString("N"));

        await bus.Publish(
            new StreamingMessage
            {
                RoutingKey = ebInstall.EdgeBoxId.ToString("N"),
                CameraId = id,
                HttpRelayUri = GetHttpUri(relayPort),
            }
        );
        return GetWebsocketUri(relayPort);
    }

    private static Uri GetHttpUri(RelayInformation relayInformation) =>
        new($"http://localhost:{relayInformation.HttpPort}/{relayInformation.Secret}");

    private static Uri GetWebsocketUri(RelayInformation relayInformation) =>
        new($"ws://localhost:{relayInformation.WebsocketPort}");
}
