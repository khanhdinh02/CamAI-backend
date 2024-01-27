using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.Domain.Entities;

namespace Core.Domain.DTO;

public class UpdateEmployeeDto
{
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? AddressLine { get; set; }
    public int? WardId { get; set; }
    public byte[]? Timestamp { get; set; }
}
