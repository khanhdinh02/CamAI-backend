namespace Core.Domain.DTO;

public class HumanCountDto
{
    public Guid ShopId { get; set; }
    public DateTime Time { get; set; }
    public int Low { get; set; }
    public int High { get; set; }
    public int Open { get; set; }
    public int Close { get; set; }
    public float Median { get; set; }
}
