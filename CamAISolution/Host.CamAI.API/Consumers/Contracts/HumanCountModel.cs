using Core.Domain.Models.Consumers;
using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("HumanCountModel")]
public class HumanCountModelContract
{
    public DateTime Time { get; set; }
    public int Total { get; set; }
    public Guid ShopId { get; set; }

    public HumanCountModel ToHumanCountModel() =>
        new()
        {
            Time = Time,
            Total = Total,
            ShopId = ShopId
        };
}
