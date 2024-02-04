using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[Publisher(ConsumerConstant.UpdateData)]
[MessageUrn(nameof(BrandUpdateMessage))]
public class BrandUpdateMessage : RoutingKeyMessage
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
