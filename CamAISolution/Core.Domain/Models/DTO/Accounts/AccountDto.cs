using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class AccountDtoWithoutBrand : BaseDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public DateOnly? Birthday { get; set; }
    public int? WardId { get; set; }
    public string? AddressLine { get; set; }
    public Guid? WorkingShopId { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public WardDto? Ward { get; set; }
    public ShopDto? WorkingShop { get; set; }
    public ShopDto? ManagingShop { get; set; }
    public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
}

public class AccountDto : AccountDtoWithoutBrand
{
    public BrandDtoWithoutBrandManager? Brand { get; set; }
}
