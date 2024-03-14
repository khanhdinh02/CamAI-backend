using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[Publisher(PublisherConstant.UpdateData)]
[MessageUrn(nameof(ShopUpdateMessage))]
public class ShopUpdateMessage : RoutingKeyMessage
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string Address { get; set; } = null!;
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
}
