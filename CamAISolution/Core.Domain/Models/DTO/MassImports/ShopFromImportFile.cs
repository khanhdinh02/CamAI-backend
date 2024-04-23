using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class ShopFromImportFile
{
    public string ShopName { get; set; } = string.Empty;
    public TimeOnly ShopOpenTime { get; set; }
    public TimeOnly ShopCloseTime { get; set; }
    public string? ShopPhone { get; set; }
    /// <summary>
    /// Shop's address line
    /// </summary>
    public string ShopAddress { get; set; } = null!;
    public string ShopManagerEmail { get; set; } = null!;
    public string ShopManagerName { get; set; } = null!;
    public Gender? ShopManagerGender { get; set; } = Gender.Male;
    /// <summary>
    /// Shop manager's address line
    /// </summary>
    public string ShopManagerAddress { get; set; } = null!;
}