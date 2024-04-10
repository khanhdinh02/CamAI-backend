using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class HumanCountDto
{
    public Guid ShopId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ReportInterval Interval { get; set; }
    public List<HumanCountItemDto> Data { get; set; } = [];
}

public class HumanCountItemDto
{
    public DateTime Time { get; set; }
    public int Low { get; set; }
    public int High { get; set; }
    public int Open { get; set; }
    public int Close { get; set; }
    public float Median { get; set; }
}
