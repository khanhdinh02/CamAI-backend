using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class HumanCountDto
{
    public Guid ShopId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ReportInterval Interval { get; set; }
    public int TotalInteraction { get; set; }
    public List<EmployeeAndInteractionDto> Data { get; set; } = [];
}

public class EmployeeAndInteractionDto
{
    public DateTime Time { get; set; }
    public HumanCountItemDto HumanCount { get; set; }
    public InteractionCountDto Interaction { get; set; }
}

public class HumanCountItemDto
{
    public int Low { get; set; }
    public int High { get; set; }
    public int Open { get; set; }
    public int Close { get; set; }
    public float Median { get; set; }
}

public class InteractionCountDto
{
    public int Count { get; set; }
    public double? AverageDuration { get; set; }
}
