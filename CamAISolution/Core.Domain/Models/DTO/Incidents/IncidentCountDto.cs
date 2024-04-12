using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class IncidentCountDto
{
    public Guid ShopId { get; set; }
    public int Total { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ReportInterval Interval { get; set; }
    public List<IncidentCountItemDto> Data { get; set; } = [];
}

public class IncidentCountItemDto(DateTime time, int count, double? averageDuration = null)
{
    public DateTime Time { get; set; } = time;
    public int Count { get; set; } = count;
    public double? AverageDuration { get; set; } = averageDuration;
}
