using Core.Domain.DTO;

namespace Core.Domain.Models.DTO.Shops;

public class SearchShopRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public int? StatusId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? ShopManagerId { get; set; }
}
