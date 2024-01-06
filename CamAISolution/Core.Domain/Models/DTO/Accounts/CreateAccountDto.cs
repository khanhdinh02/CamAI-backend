using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Core.Domain.Entities;

namespace Core.Domain.DTO;

public class CreateAccountDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }

    [StringLength(50)]
    public string Phone { get; set; } = null!;
    public DateOnly Birthday { get; set; }
    public Guid? WardId { get; set; }
    public string? AddressLine { get; set; } = null!;
    public Guid? BrandId { get; set; }
    public Guid? WorkingShopId { get; set; }
    public ICollection<int> RoleIds { get; set; } = new HashSet<int>();
}
