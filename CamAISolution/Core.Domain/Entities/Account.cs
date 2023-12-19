using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Account : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public Guid? GenderId { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public Guid? WardId { get; set; }
    public string? AddressLine { get; set; }

    /// <summary>
    /// Brand that the brand manager is managing.
    /// <c>null</c> if the account is not a brand manager.
    /// </summary>
    public Guid? BrandId { get; set; }

    /// <summary>
    /// Shop that the shop manager is managing or the employee is working.
    /// <c>null</c> if the account is not a shop manager or an employee.
    /// </summary>
    public Guid? ShopId { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = Statuses.Inactive;

    public virtual Gender? Gender { get; set; }
    public virtual Ward? Ward { get; set; }
    public virtual Brand? Brand { get; set; }
    public virtual Shop? Shop { get; set; }
    public virtual ICollection<AccountRole> AccountRoles { get; set; } = new HashSet<AccountRole>();

    public static class Statuses
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
    }
}
