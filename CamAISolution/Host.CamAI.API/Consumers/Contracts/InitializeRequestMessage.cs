using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("InitializeRequest")]
public class InitializeRequestMessage
{
    public Guid EdgeBoxId { get; set; }
    public string MacAddress { get; set; } = null!;
    public string Version { get; set; } = null!;
    public string OperatingSystem { get; set; } = null!;
    public string IpAddress { get; set; } = null!;
    public string SerialNumber {get;set; } = null!;
}
