using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum Role
{
    Admin = 1,
    BrandManager = 3,
    ShopManager = 4,
    SystemHandler = 5
}
