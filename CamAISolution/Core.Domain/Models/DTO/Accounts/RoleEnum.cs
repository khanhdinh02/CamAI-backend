using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
public static class RoleEnum
{
    public const int Admin = 1;
    public const int Technician = 2;
    public const int BrandManager = 3;
    public const int ShopManager = 4;
    public const int Employee = 5;
}
