using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class EdgeBoxInstall : BusinessEntity
{
    public Guid EdgeBoxId { get; set; }
    public Guid ShopId { get; set; }
    public string? ActivationCode { get; set; }
    public DateTime? UninstalledTime { get; set; }
    public EdgeBoxActivationStatus ActivationStatus { get; set; }
    public EdgeBoxInstallStatus EdgeBoxInstallStatus { get; set; }

    public DateTime? LastSeen { get; set; }
    public string? IpAddress { get; set; }
    public string? OperatingSystem { get; set; }

    public virtual EdgeBox EdgeBox { get; set; } = null!;
    public virtual Shop Shop { get; set; } = null!;
}
