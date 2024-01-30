namespace Core.Domain.Models.Consumers;

public class HumanCount
{
    public DateTime Time { get; set; }
    public int Count { get; set; }
    public Guid ShopId { get; set; }
}
