using Core.Application.Exceptions;
using Core.Domain.Interfaces.Services;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Infrastructure.Streaming;

public class StreamingService(
    ICameraService cameraService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IPublishEndpoint bus,
    IOptions<StreamingConfiguration> confOpt
) : IStreamingService
{
    private readonly StreamingConfiguration configuration = confOpt.Value;

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

    private Uri GetHttpUri(RelayInformation relayInformation) =>
        new($"http://{configuration.StreamingReceiveDomain}:{relayInformation.HttpPort}/{relayInformation.Secret}");

    private Uri GetWebsocketUri(RelayInformation relayInformation) =>
        new($"wss://{configuration.StreamingDomain}/{relayInformation.WebsocketPort}/{relayInformation.Secret}");
}
