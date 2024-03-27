using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchEdgeBoxInstallRequest : BaseSearchRequest
{
    public Guid? EdgeBoxId { get; set; }
    public EdgeBoxInstallStatus? EdgeBoxInstallStatus { get; set; }
}
