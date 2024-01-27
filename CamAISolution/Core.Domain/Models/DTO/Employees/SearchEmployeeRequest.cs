namespace Core.Domain.DTO;

public class SearchEmployeeRequest : BaseSearchRequest
{
    /// <summary>
    /// Name, Email, or Phone
    /// </summary>
    public string? Search { get; set; }
    public int? EmployeeStatusId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? ShopId { get; set; }
}
