using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EdgeBoxInstallActivityDto
{
    public Guid Id { get; set; }
    public Guid ModifiedById { get; set; }
    public DateTime ModifiedTime { get; set; }
    public Guid EdgeBoxInstallId { get; set; }
    public EdgeBoxInstallStatus OldStatus { get; set; }
    public EdgeBoxInstallStatus NewStatus { get; set; }
}
