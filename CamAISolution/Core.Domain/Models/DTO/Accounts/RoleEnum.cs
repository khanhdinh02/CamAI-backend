namespace Core.Domain.Models.DTO.Accounts;

public static class RoleEnum
{
    //Role
    public static readonly Guid Admin = Guid.Parse("2381d027-707a-41ee-b53a-26e967b78d75");
    public static readonly Guid Technician = Guid.Parse("658c20e3-b4e8-46ce-a8b7-a9e1860c9ff8");
    public static readonly Guid BrandManager = Guid.Parse("80bf167e-7411-4727-a508-76415eb7bfbd");
    public static readonly Guid ShopManager = Guid.Parse("e991447e-723d-4a21-9779-7b0a6ea33998");
    public static readonly Guid Employee = Guid.Parse("c6e17622-a67a-4bf9-b12c-2761d4768d85");
}
