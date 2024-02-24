using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class ShopDto : BaseDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }
    public int WardId { get; set; }
    public string? AddressLine { get; set; }
    public ShopStatus ShopStatus { get; set; }

    public WardDto Ward { get; set; } = null!;
    public BrandDto Brand { get; set; } = null!;
}
