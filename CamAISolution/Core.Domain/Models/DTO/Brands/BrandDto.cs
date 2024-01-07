namespace Core.Domain.DTO;

public class BrandDto : BaseDto
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Uri? LogoUri { get; set; }
    public Uri? BannerUri { get; set; }
    public Guid? BrandManagerId { get; set; }
    public BaseStatusDto BrandStatus { get; set; } = null!;
}
