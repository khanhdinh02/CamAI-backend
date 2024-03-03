using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class UpdateAccountDto
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public Gender Gender { get; set; }

    [StringLength(50)]
    public string Phone { get; set; } = null!;
    public DateOnly Birthday { get; set; }
    public int? WardId { get; set; }
    public string? AddressLine { get; set; } = null!;
    public virtual byte[]? Timestamp { get; set; }
}
