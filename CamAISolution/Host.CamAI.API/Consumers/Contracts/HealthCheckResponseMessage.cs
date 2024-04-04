using Core.Domain.Enums;
using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("HealthCheckResponseMessage")]
public class HealthCheckResponseMessage
{
    public Guid EdgeBoxId { get; set; }
    public EdgeBoxInstallStatus Status { get; set; }
    public string? Reason { get; set; }
}
