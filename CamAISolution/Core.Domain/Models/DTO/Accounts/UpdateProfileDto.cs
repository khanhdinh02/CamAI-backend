using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class UpdateProfileDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public int? WardId { get; set; }
    public string? AddressLine { get; set; } = null!;
    public virtual byte[]? Timestamp { get; set; }
}
