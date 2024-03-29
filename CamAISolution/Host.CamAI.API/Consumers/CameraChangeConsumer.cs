using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;
using Action = Host.CamAI.API.Consumers.Contracts.Action;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_Camera", ConsumerConstant.CameraChange)]
public class CameraChangeConsumer(ICameraService cameraService) : IConsumer<CameraChangeMessage>
{
    public async Task Consume(ConsumeContext<CameraChangeMessage> context)
    {
        var message = context.Message;
        switch (message.Action)
        {
            case Action.Upsert:
                await cameraService.UpsertCameraForRoleEdgeBox(ToCamera(message.Camera));
                break;
            case Action.Delete:
                await cameraService.DeleteCameraForRoleEdgeBox(message.Camera.Id);
                break;
        }
    }

    private static Camera ToCamera(EdgeBoxCameraDto cameraDto) =>
        new()
        {
            Id = cameraDto.Id,
            ShopId = cameraDto.ShopId,
            Name = cameraDto.Name,
            Zone = cameraDto.Zone
        };
}
