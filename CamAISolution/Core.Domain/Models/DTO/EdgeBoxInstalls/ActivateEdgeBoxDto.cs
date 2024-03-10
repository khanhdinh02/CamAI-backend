namespace Core.Domain.DTO;

public class ActivateEdgeBoxDto
{
    public Guid ShopId { get; set; }
    public string ActivationCode { get; set; } = null!;
}
