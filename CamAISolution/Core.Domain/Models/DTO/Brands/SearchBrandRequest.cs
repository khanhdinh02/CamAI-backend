using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchBrandRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public BrandStatus? BrandStatus { get; set; }
    public Guid? BrandId { get; set; }
    public bool? HasManager { get; set; }
}
