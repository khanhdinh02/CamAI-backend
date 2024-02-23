using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Employee : BusinessEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public Gender Gender { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public Uri? Image { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? AddressLine { get; set; }
    public int? WardId { get; set; }
    public Guid? ShopId { get; set; }
    public EmployeeStatus EmployeeStatus { get; set; }

    public virtual Ward? Ward { get; set; }
    public virtual Shop? Shop { get; set; }
    public virtual ICollection<Behavior> Behaviors { get; set; } = new HashSet<Behavior>();
}
