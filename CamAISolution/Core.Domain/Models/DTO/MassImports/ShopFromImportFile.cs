using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class ShopFromImportFile
{
    public string? ExternalShopId { get; set; }
    public string ShopName { get; set; } = string.Empty;
    public TimeOnly ShopOpenTime { get; set; }
    public TimeOnly ShopCloseTime { get; set; }
    public string? ShopPhone { get; set; }
    /// <summary>
    /// Shop's address line
    /// </summary>
    public string ShopAddress { get; set; } = null!;
    public string? ExternalShopManagerId { get; set; }
    public string ShopManagerEmail { get; set; } = null!;
    public string ShopManagerName { get; set; } = null!;
    public Gender? ShopManagerGender { get; set; } = Gender.Male;
    /// <summary>
    /// Shop manager's address line
    /// </summary>
    public string ShopManagerAddress { get; set; } = null!;

    public Account GetManager() => new Account
    {
        Email = ShopManagerEmail,
        Name = ShopManagerName,
        Gender = ShopManagerGender,
        AddressLine = ShopManagerAddress,
        ExternalId = ExternalShopManagerId,
    };

    public Shop GetShop() => new Shop
    {
        ExternalId = ExternalShopId,
        Name = ShopName,
        OpenTime = ShopOpenTime,
        CloseTime = ShopCloseTime,
        Phone = ShopPhone,
        AddressLine = ShopAddress
    };
}