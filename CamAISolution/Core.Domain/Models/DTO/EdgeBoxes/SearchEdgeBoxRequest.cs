using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchEdgeBoxRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public EdgeBoxStatus? EdgeBoxStatus { get; set; }
    public EdgeBoxLocation? EdgeBoxLocation { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? ShopId { get; set; }
}
