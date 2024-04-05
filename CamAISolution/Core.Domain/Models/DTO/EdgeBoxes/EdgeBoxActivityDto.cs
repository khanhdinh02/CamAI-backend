using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EdgeBoxActivityDto : BaseActivityDto
{
    public Guid? EdgeBoxId { get; set; }
    public Guid? EdgeBoxInstallId { get; set; }
    public EdgeBoxActivityType Type { get; set; }
}
