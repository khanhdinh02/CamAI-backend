using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

public class CreateShopDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [Phone, MaxLength(50)]
    public string? Phone { get; set; }
    public Guid WardId { get; set; }
    public string? AddressLine { get; set; }
}
