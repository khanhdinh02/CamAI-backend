using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.Domain.Entities;

namespace Core.Domain.DTO;

public class UpdateAccountDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }

    [StringLength(50)]
    public string Phone { get; set; } = null!;
    public DateOnly Birthday { get; set; }
    public Guid? WardId { get; set; }
    public string? AddressLine { get; set; } = null!;
    public virtual byte[]? Timestamp { get; set; }
}
