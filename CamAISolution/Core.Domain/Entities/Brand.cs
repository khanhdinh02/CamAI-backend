using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Brand : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = Statuses.Inactive;

    public static class Statuses
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
    }
}
