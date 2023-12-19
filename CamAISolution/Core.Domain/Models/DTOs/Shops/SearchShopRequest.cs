namespace Core.Domain;

public class SearchShopRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public string? Status { get; set; }
}
