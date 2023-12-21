using Core.Domain.Models.DTOs.Base;

namespace Core.Domain.Models.DTOs.Brands;

public class SearchBrandRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public Guid? StatusId { get; set; }
}
