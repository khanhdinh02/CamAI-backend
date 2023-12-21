using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class AccountStatus : BaseEntity
{
    [StringLength(20)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
