using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EdgeBoxInstallReportDto
{
    public int Total { get; set; }
    public Dictionary<EdgeBoxInstallStatus, int> Status { get; set; } = null!;
    public Dictionary<EdgeBoxActivationStatus, int> ActivationStatus { get; set; } = null!;
}
