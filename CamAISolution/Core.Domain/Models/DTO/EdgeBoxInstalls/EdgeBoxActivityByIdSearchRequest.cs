namespace Core.Domain.DTO;

public class EdgeBoxActivityByIdSearchRequest : BaseSearchRequest
{
    public Guid? EdgeBoxInstallId { get; set; }
}
