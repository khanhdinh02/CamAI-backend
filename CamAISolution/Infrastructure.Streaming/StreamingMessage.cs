using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Streaming;

[Publisher(PublisherConstant.Streaming)]
[MessageUrn("StreamingMessage")]
public class StreamingMessage : RoutingKeyMessage
{
    public Guid CameraId { get; set; }
    public Uri? HttpRelayUri { get; set; }
}

