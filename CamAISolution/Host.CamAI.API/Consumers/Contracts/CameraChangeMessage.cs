using Core.Domain.Enums;
using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("CameraChangeMessage")]
public class CameraChangeMessage
{
    public EdgeBoxCameraDto Camera { get; set; } = null!;
    public Action Action { get; set; }
}

public class EdgeBoxCameraDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid ShopId { get; set; }
    public Zone Zone { get; set; }
}

public enum Action
{
    Upsert = 1,
    Delete = 2,
}
