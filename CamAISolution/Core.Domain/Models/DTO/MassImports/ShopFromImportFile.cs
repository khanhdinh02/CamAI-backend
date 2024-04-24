using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Core.Domain.Constants;
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
    [EmailAddress]
    public string ShopManagerEmail { get; set; } = null!;
    public string ShopManagerName { get; set; } = null!;
    public Gender? ShopManagerGender { get; set; } = Gender.Male;
    /// <summary>
    /// Shop manager's address line
    /// </summary>
    public string ShopManagerAddress { get; set; } = null!;

    private IDictionary<string, object?> ShopValidation()
    {
        var result = new Dictionary<string, object?>();
        if (ShopOpenTime >= ShopCloseTime)
            result.Add($"{nameof(ShopOpenTime)}", "Shop open time cannot later or equal than close time");
        if (ShopName.Length > 50)
            result.Add($"{nameof(ShopName)}", "Shop name's length must be less than or equal to 50");
        if (ShopPhone != null && !RegexHelper.VietNamPhoneNumber.IsMatch(ShopPhone))
            result.Add($"{nameof(ShopPhone)}", $"{ShopPhone} is wrong");
        return result;
    }

    private IDictionary<string, object?> AccountValidation()
    {
        var result = new Dictionary<string, object?>();
        if (!MailAddress.TryCreate(ShopManagerEmail, out _))
            result.Add($"{nameof(ShopManagerEmail)}", $"{ShopManagerEmail} is wrong format");
        if (ShopManagerName.Length > 50)
            result.Add($"{nameof(ShopManagerName)}", "Manager name's length must be less than or equal to 50");
        return result;
    }

    public bool IsValid() => !ShopFromImportFileValidation().Any();

    public IDictionary<string, object?> ShopFromImportFileValidation() =>
        AccountValidation().Aggregate(ShopValidation(), (aggregateDict, next) =>
        {
            aggregateDict[next.Key] = next.Value;
            return aggregateDict;
        });
    public Account GetManager() => new()
    {
        Email = ShopManagerEmail,
        Name = ShopManagerName,
        Gender = ShopManagerGender,
        AddressLine = ShopManagerAddress,
        ExternalId = ExternalShopManagerId,
    };

    public Shop GetShop() => new()
    {
        ExternalId = ExternalShopId,
        Name = ShopName,
        OpenTime = ShopOpenTime,
        CloseTime = ShopCloseTime,
        Phone = ShopPhone,
        AddressLine = ShopAddress
    };
}