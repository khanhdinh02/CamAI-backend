using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchEdgeBoxRequest : BaseSearchRequest
{
    public string? Model { get; set; }
    public EdgeBoxStatus? EdgeBoxStatus { get; set; }
    public EdgeBoxLocation? EdgeBoxLocation { get; set; }
}
