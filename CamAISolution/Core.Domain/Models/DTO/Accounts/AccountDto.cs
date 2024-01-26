using System.Text.Json.Serialization;
using Core.Domain.Entities;

namespace Core.Domain.DTO;

public class AccountDtoWithoutBrand : BaseDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public int? WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? WorkingShopId { get; set; }
    public int AccountStatusId { get; set; }
    public WardDto? Ward { get; set; }
    public ShopDto? WorkingShop { get; set; }
    public LookupDto AccountStatus { get; set; } = null!;
    public ShopDto? ManagingShop { get; set; }
    public ICollection<LookupDto> Roles { get; set; } = new HashSet<LookupDto>();
    public ICollection<NotificationDto> SentNotifications { get; set; } = new HashSet<NotificationDto>();
}

public class AccountDto : AccountDtoWithoutBrand
{
    public BrandDtoWithoutBrandManager? Brand { get; set; }
}
