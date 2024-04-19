using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EdgeBoxReportDto
{
    public int Total { get; set; }
    public Dictionary<EdgeBoxStatus, int> Status { get; set; } = null!;
    public Dictionary<EdgeBoxLocation, int> Location { get; set; } = null!;
}
