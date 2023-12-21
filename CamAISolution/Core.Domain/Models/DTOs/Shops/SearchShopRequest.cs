using Core.Domain.Models.DTOs.Base;

namespace Core.Domain.Models.DTOs.Shops;

public class SearchShopRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public Guid? StatusId { get; set; }
}
