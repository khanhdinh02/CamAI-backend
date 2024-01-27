using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class ImageTypeEnum
{
    public const int BrandBanner = 1;
    public const int BrandLogo = 2;
    public const int AccountAvatar = 3;
}
