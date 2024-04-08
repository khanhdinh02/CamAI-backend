using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Infrastructure.Streaming;

public class StreamingService(
    ICameraService cameraService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IPublishEndpoint bus,
    IOptions<StreamingConfiguration> confOpt,
    IAppLogging<StreamingService> logger
) : IStreamingService
{
    private readonly StreamingConfiguration configuration = confOpt.Value;

    public async Task<Uri> StreamCamera(Guid id)
    {
        logger.Info($"Receive streaming request for camera {id}");
        var camera = await cameraService.GetCameraById(id);
        var ebInstall =
            await edgeBoxInstallService.GetCurrentInstallationByShop(camera.ShopId)
            ?? throw new BadRequestException("There is not edge box installed in shop");

        switch (camera.Status)
        {
            case CameraStatus.Disabled:
                throw new BadRequestException("Camera is disabled");
            case CameraStatus.Disconnected:
                throw new BadRequestException("Camera is disconnected. Please contact admin for support");
        }

        var relayPort = WebsocketRelayProcessManager.Run(camera.Id.ToString("N"));

        logger.Info($"Publish streaming request to edge box {ebInstall.EdgeBoxId} for camera {id}");
        await bus.Publish(
            new StreamingMessage
            {
                RoutingKey = ebInstall.EdgeBoxId.ToString("N"),
                CameraId = id,
                HttpRelayUri = GetHttpUri(relayPort),
            }
        );
        logger.Info($"Return web socket uri {GetWebsocketUri(relayPort)}");
        return GetWebsocketUri(relayPort);
    }

    private Uri GetHttpUri(RelayInformation relayInformation) =>
        new($"http://{configuration.StreamingReceiveDomain}:{relayInformation.HttpPort}/{relayInformation.Secret}");

    private Uri GetWebsocketUri(RelayInformation relayInformation) =>
        new($"wss://{configuration.StreamingDomain}/{relayInformation.WebsocketPort}/{relayInformation.Secret}");
}
