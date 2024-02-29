using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class ShopSearchRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public int? WardId { get; set; }
    public ShopStatus? Status { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? ShopManagerId { get; set; }
}
