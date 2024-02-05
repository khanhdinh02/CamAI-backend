using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class EdgeBox : BusinessEntity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string? Model { get; set; }

    public string? Version { get; set; }

    // TODO [Duy]: Edge box configuration
    public int EdgeBoxStatusId { get; set; }
    public int EdgeBoxLocationId { get; set; }
    public string HostingAddress { get; set; } = null!;

    public virtual EdgeBoxLocation EdgeBoxLocation { get; set; } = null!;
    public virtual EdgeBoxStatus EdgeBoxStatus { get; set; } = null!;
    public virtual ICollection<EdgeBoxInstall> Installs { get; set; } = new HashSet<EdgeBoxInstall>();
}
