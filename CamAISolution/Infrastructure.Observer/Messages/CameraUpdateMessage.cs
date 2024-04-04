using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[Publisher(PublisherConstant.UpdateData)]
[MessageUrn(nameof(CameraUpdateMessage))]
public class CameraUpdateMessage : RoutingKeyMessage
{
    public List<EdgeBoxCameraDto> Cameras { get; set; } = null!;
}
