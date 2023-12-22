using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Camera : BaseEntity
{
    [StringLength(100)]
    public string Name { get; set; } = null!;
    public Guid ShopId { get; set; }
    public Guid? EdgeBoxId { get; set; }

    public virtual Shop Shop { get; set; } = null!;
    public virtual EdgeBox? EdgeBox { get; set; }
}
