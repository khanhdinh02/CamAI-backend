using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Camera : BusinessEntity
{
    [StringLength(255)]
    public string Name { get; set; } = null!;
    public Guid ShopId { get; set; }
    public Zone Zone { get; set; }

    [StringLength(255)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    [StringLength(10)]
    public string Protocol { get; set; } = null!;
    public int Port { get; set; }
    public string Host { get; set; } = null!;
    public string Path { get; set; } = null!;
    public bool WillRunAI { get; set; }

    public CameraStatus Status { get; set; } = CameraStatus.New;
    public virtual Shop Shop { get; set; } = null!;
}
