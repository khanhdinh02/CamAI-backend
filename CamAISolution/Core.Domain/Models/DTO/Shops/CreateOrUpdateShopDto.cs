using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class CreateOrUpdateShopDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [Phone, MaxLength(50)]
    public string? Phone { get; set; }
    public Guid WardId { get; set; }
    public Guid BrandId { get; set; }
    public Guid? ShopManagerId { get; set; }
    public string? AddressLine { get; set; }
}