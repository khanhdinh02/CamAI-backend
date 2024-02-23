using Core.Domain.Models.Attributes;

namespace Core.Domain.Enums;

[Lookup]
public enum Role
{
    Admin = 1,
    Technician,
    BrandManager,
    ShopManager,
    Employee
}
