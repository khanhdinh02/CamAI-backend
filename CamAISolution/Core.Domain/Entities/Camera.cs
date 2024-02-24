using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Camera : BusinessEntity
{
    [StringLength(100)]
    public string Name { get; set; } = null!;
    public Guid ShopId { get; set; }

    public virtual Shop Shop { get; set; } = null!;
}
