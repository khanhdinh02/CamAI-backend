using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;

namespace Core.Domain.DTO;

public class ShopDto : BaseDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [MaxLength(50)]
    public string? Phone { get; set; }
    public Guid WardId { get; set; }
    public string? AddressLine { get; set; }

    public WardDto Ward { get; set; } = null!;
    public BrandDto Brand { get; set; } = null!;
    public ShopStatus ShopStatus { get; set; } = null!;
}
