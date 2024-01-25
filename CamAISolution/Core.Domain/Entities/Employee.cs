using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Employee : BusinessEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Gender Gender { get; set; }
    public string? Phone { get; set; }
    public Uri? Image { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? AddressLine { get; set; }
    public int? WardId { get; set; }
    public Guid ShopId { get; set; }
    public int EmployeeStatusId { get; set; }

    public virtual Ward? Ward { get; set; }
    public virtual Shop Shop { get; set; } = null!;
    public virtual EmployeeStatus EmployeeStatus { get; set; } = null!;
}
