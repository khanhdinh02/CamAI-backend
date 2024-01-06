namespace Core.Domain.DTO;

public class SearchEdgeBoxRequest : BaseSearchRequest
{
    public string? Model { get; set; }
    public int? EdgeBoxStatusId { get; set; }
    public int? EdgeBoxLocationId { get; set; }
}
