namespace Core.Domain.DTO;

public class SearchShopRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public Guid? StatusId { get; set; }
}
