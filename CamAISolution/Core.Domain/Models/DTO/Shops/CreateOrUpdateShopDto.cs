using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class CreateOrUpdateShopDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }

    [Phone, MaxLength(50)]
    public string? Phone { get; set; }
    public int WardId { get; set; }
    public Guid? ShopManagerId { get; set; }
    public string AddressLine { get; set; } = null!;
}
