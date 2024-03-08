using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EdgeBoxInstallDto : BaseDto
{
    public Guid EdgeBoxId { get; set; }
    public Guid ShopId { get; set; }
    public string? IpAddress { get; set; } = null!;
    public int? Port { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public EdgeBoxInstallSubscription EdgeBoxInstallSubscription { get; set; }

    public EdgeBoxDto EdgeBox { get; set; } = null!;
    public ShopDto Shop { get; set; } = null!;
}
