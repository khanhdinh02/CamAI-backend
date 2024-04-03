using Infrastructure.Observer.Messages;
using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("CameraChangeMessage")]
public class CameraChangeMessage
{
    public EdgeBoxCameraDto Camera { get; set; } = null!;
    public Action Action { get; set; }
}

public enum Action
{
    Upsert = 1,
    Delete = 2,
}
