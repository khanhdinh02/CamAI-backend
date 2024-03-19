using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Camera : BusinessEntity
{
    [StringLength(100)]
    public string Name { get; set; } = null!;
    public Guid ShopId { get; set; }
    public Zone Zone { get; set; }
    public CameraStatus Status { get; set; } = CameraStatus.Active;
    public virtual Shop Shop { get; set; } = null!;
}
