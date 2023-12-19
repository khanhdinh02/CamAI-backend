using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Shop : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Phone { get; set; }
    public Guid WardId { get; set; }
    public string? AddressLine { get; set; }

    public virtual Ward Ward { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = Statuses.Inactive;

    public static class Statuses
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
    }
}
