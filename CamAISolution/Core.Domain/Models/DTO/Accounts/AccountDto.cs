using System.Text.Json.Serialization;
using Core.Domain.Entities;

namespace Core.Domain.DTO;

public class AccountDto : BaseDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public Guid? WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? WorkingShopId { get; set; }
    public int AccountStatusId { get; set; }
    public WardDto? Ward { get; set; }
    public ShopDto? WorkingShop { get; set; }

    // TODO [Khanh]: BaseStatusDto -> LookupDto
    public BaseStatusDto AccountStatus { get; set; } = null!;
    public BrandDto? Brand { get; set; }
    public ShopDto? ManagingShop { get; set; }
    public ICollection<BaseStatusDto> Roles { get; set; } = new HashSet<BaseStatusDto>();
}
