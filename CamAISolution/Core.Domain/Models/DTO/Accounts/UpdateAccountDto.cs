using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class UpdateAccountDto
{
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = null!;
    public Gender Gender { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public int? WardId { get; set; }
    public string? AddressLine { get; set; }
    public byte[]? Timestamp { get; set; }
}
