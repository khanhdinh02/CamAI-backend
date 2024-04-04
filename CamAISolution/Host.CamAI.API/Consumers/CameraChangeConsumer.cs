using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using Infrastructure.Observer.Messages;
using MassTransit;
using Action = Host.CamAI.API.Consumers.Contracts.Action;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_Camera", ConsumerConstant.CameraChange)]
public class CameraChangeConsumer(ICameraService cameraService, IBaseMapping mapper) : IConsumer<CameraChangeMessage>
{
    public async Task Consume(ConsumeContext<CameraChangeMessage> context)
    {
        var message = context.Message;
        switch (message.Action)
        {
            case Action.Upsert:
                await cameraService.UpsertCamera(mapper.Map<EdgeBoxCameraDto, Camera>(message.Camera));
                break;
            case Action.Delete:
                await cameraService.DeleteCamera(message.Camera.Id);
                break;
        }
    }
}
