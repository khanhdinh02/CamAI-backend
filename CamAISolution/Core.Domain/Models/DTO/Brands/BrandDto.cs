using Core.Domain.Enums;
using Core.Domain.Models.DTO;

namespace Core.Domain.DTO;

public class BrandDto : BaseDto
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public ImageDto? Logo { get; set; }
    public ImageDto? Banner { get; set; }
    public Guid? BrandManagerId { get; set; }
    public BrandStatus BrandStatus { get; set; }
    public AccountDto? BrandManager { get; set; }
}
