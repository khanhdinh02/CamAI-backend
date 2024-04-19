using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchEdgeBoxInstallRequest : BaseSearchRequest
{
    public Guid? EdgeBoxId { get; set; }
    public EdgeBoxLocation? EdgeBoxLocation { get; set; }
    public EdgeBoxInstallStatus? EdgeBoxInstallStatus { get; set; }
    public Guid? ShopId { get; set; }
    public EdgeBoxActivationStatus? ActivationStatus { get; set; }

    public DateTime? StartLastSeen { get; set; }
    public DateTime? EndLastSeen { get; set; }
}
