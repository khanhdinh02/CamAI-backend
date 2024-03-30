namespace Core.Domain.DTO;

public class EdgeBoxActivityByEdgeBoxIdSearchRequest : BaseSearchRequest
{
    public Guid? EdgeBoxId { get; set; }
}
