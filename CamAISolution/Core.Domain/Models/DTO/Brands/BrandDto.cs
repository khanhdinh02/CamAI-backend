namespace Core.Domain.DTO;

public class BrandDtoWithoutBrandManager : BaseDto
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Uri? LogoUri { get; set; }
    public Uri? BannerUri { get; set; }
    public Guid? BrandManagerId { get; set; }
    public LookupDto BrandStatus { get; set; } = null!;
}

public class BrandDto : BrandDtoWithoutBrandManager
{
    public AccountDtoWithoutBrand? BrandManager { get; set; }
}
