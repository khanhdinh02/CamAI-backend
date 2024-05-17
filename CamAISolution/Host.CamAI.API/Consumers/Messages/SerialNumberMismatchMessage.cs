using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers.Messages;

[MessageUrn("SerialNumberMismatchMessage")]
[Publisher(PublisherConstant.SerialNumberMismatch)]
public class SerialNumberMismatchMessage : RoutingKeyMessage
{
    public string SerialNumber { get; set; } = null!;
}
