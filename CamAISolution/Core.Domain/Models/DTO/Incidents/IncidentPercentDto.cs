using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class IncidentPercentDto
{
    public Guid ShopId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Total { get; set; }
    public List<IncidentStatusPercentDto> Statuses { get; set; } = [];
    public List<IncidentTypePercentDto> Types { get; set; } = [];
}

public class IncidentTypePercentDto
{
    public IncidentType Type { get; set; }
    public int Total { get; set; }
    public double Percent { get; set; }
    public List<IncidentStatusPercentDto> Statuses { get; set; } = [];
}

public class IncidentStatusPercentDto
{
    public IncidentStatus Status { get; set; }
    public int Total { get; set; }
    public double Percent { get; set; }
}
