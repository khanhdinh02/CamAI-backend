using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class EdgeBox : BusinessEntity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string? Model { get; set; }
    public int EdgeBoxStatusId { get; set; }

    public virtual EdgeBoxStatus EdgeBoxStatus { get; set; } = null!;
    public virtual ICollection<EdgeBoxInstall> Installs { get; set; } = new HashSet<EdgeBoxInstall>();
}
