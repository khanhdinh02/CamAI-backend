namespace Core.Domain.DTOs;

public class SearchShopRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public Guid? StatusId { get; set; }
}
