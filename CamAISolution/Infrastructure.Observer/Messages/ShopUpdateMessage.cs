using Infrastructure.MessageQueue;
using MassTransit;

namespace Infrastructure.Observer.Messages;

[MessageUrn(nameof(ShopUpdateMessage))]
public class ShopUpdateMessage : RoutingKeyMessage
{
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string Address { get; set; } = null!;
}
