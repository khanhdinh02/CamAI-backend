using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class EdgeBoxInstall : BaseEntity
{
    public Guid EdgeBoxId { get; set; }
    public Guid ShopId { get; set; }

    [StringLength(50)]
    public string IpAddress { get; set; } = null!;
    public int Port { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public Guid EdgeBoxInstallStatusId { get; set; }

    public virtual EdgeBox EdgeBox { get; set; } = null!;
    public virtual Shop Shop { get; set; } = null!;
    public virtual EdgeBoxInstallStatus EdgeBoxInstallStatus { get; set; } = null!;
}
