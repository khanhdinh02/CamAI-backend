namespace Core.Domain;

public class AppConstant
{
    //Account Status
    public static Guid AccountNewStatus = Guid.Parse("3a62b31e-ea0f-4b8d-8f37-8bb9da899c23");
    public static Guid AccountActiveStatus = Guid.Parse("f4468b33-ee55-4e34-898d-7ec37db36ca0");
    public static Guid AccountInactiveStatus = Guid.Parse("daf87982-2b47-494c-9266-28c6679c77f0");

    //Brand
    public static Guid BrandActiveStatus = Guid.Parse("79f5bf99-dd0c-4787-924b-c7f175615054");
    public static Guid BrandInactiveStatus = Guid.Parse("cc835da5-7954-4873-a029-4b472542bb3a");

    //Shop status
    public static Guid ShopActiveStatus = Guid.Parse("1b4cf615-ffe6-4416-9450-42a11554db10");
    public static Guid ShopInactiveStatus = Guid.Parse("9eb5c65c-3a69-459e-9ff5-1f0c15353c57");

    //Role
    public static Guid RoleAdmin = Guid.Parse("2381d027-707a-41ee-b53a-26e967b78d75");
    public static Guid RoleTenician = Guid.Parse("658c20e3-b4e8-46ce-a8b7-a9e1860c9ff8");
    public static Guid RoleBrandManager = Guid.Parse("80bf167e-7411-4727-a508-76415eb7bfbd");
    public static Guid RoleShopManager = Guid.Parse("e991447e-723d-4a21-9779-7b0a6ea33998");
    public static Guid RoleEmployee = Guid.Parse("c6e17622-a67a-4bf9-b12c-2761d4768d85");

    //Gender
    public static Guid GenderMale = Guid.Parse("08a14c6c-edc4-4c56-aa18-a847d3b39a07");
    public static Guid GenderFemale = Guid.Parse("6caf2cdd-6ab8-452c-af23-959c2fbe99c7");
}