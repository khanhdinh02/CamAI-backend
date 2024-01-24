namespace Core.Domain.DTO;

public class SearchBrandRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public int? StatusId { get; set; }
    public Guid? BrandId { get; set; }
    public bool? HasManager { get; set; }
}
