namespace Core.Domain.DTOs;

public class SearchBrandRequest : BaseSearchRequest
{
    public string? Name { get; set; }
    public Guid? StatusId { get; set; }
}
