using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTO;

public class ShopDto : BaseDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [MaxLength(50)]
    public string? Phone { get; set; }
    public int WardId { get; set; }
    public string? AddressLine { get; set; }

    public WardDto Ward { get; set; } = null!;
    public BrandDto Brand { get; set; } = null!;
    public LookupDto ShopStatus { get; set; } = null!;
}
