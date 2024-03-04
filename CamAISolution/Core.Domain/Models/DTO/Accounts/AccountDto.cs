using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class AccountDto : BaseDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public int? WardId { get; set; }
    public string? AddressLine { get; set; }
    public Role Role { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public BrandDto? Brand { get; set; }
    public ShopDto? ManagingShop { get; set; }

    public WardDto? Ward { get; set; }
}
