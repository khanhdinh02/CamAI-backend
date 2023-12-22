using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class EdgeBox : BaseEntity
{
    [StringLength(50)]
    public string Address { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid EdgeBoxStatusId { get; set; }

    public virtual EdgeBoxStatus EdgeBoxStatus { get; set; } = null!;
    public virtual ICollection<EdgeBoxInstall> Installs { get; set; } = new HashSet<EdgeBoxInstall>();
    public virtual ICollection<Camera> Cameras { get; set; } = new HashSet<Camera>();
}
