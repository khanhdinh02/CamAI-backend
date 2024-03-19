using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[Publisher(PublisherConstant.UpdateData)]
[MessageUrn(nameof(BrandUpdateMessage))]
public class BrandUpdateMessage : RoutingKeyMessage
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
