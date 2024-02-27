using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Shop : BusinessEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public int WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? ShopManagerId { get; set; }
    public Guid BrandId { get; set; }
    public ShopStatus ShopStatus { get; set; }

    public virtual Account? ShopManager { get; set; }
    public virtual Ward Ward { get; set; } = null!;
    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    public virtual ICollection<Camera> Cameras { get; set; } = new HashSet<Camera>();
    public virtual ICollection<EdgeBoxInstall> EdgeBoxInstalls { get; set; } = new HashSet<EdgeBoxInstall>();
}
