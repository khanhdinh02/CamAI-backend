using Core.Domain.Models.Consumers;
using MassTransit;

namespace Host.CamAI.API.Consumers.Contracts;

[MessageUrn("ClassifierModel")]
public class ClassifierModelContract
{
    public DateTime Time { get; set; }
    public List<ClassifierResult> Results { get; set; } = null!;
    public int Total { get; set; }
    public Guid ShopId { get; set; }

    public ClassifierModel ToClassifierModel() =>
        new()
        {
            Time = Time,
            Results = Results,
            Total = Total,
            ShopId = ShopId
        };
}
