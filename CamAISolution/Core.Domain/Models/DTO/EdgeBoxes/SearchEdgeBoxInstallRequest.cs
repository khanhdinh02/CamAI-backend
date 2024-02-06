namespace Core.Domain.DTO;

public class SearchEdgeBoxInstallRequest
{
    public int EdgeBoxInstallStatus { get; set; } = EdgeBoxInstallStatusEnum.Valid;
    public int? EdgeBoxStatus { get; set; }
}
