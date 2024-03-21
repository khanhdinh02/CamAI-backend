namespace Core.Domain.DTO;

public class CreateEdgeBoxInstallDto
{
    public Guid EdgeBoxId { get; set; }
    public Guid ShopId { get; set; }
    public string? IpAddress { get; set; } = null!;
    public int? Port { get; set; }
}
