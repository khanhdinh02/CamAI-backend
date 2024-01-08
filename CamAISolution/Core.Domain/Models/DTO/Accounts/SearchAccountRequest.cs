namespace Core.Domain.DTO;

public class SearchAccountRequest : BaseSearchRequest
{
    /// <summary>
    /// Name, Email, or Phone
    /// </summary>
    public string? Search { get; set; }
    public int? AccountStatusId { get; set; }
    public int? RoleId { get; set; }

    /// <summary>
    /// The result includes Brand manager, Shop managers, and Employees
    /// </summary>
    public Guid? BrandId { get; set; }

    /// <summary>
    /// The result includes Brand manager, Shop managers, and Employees
    /// </summary>
    public Guid? ShopId { get; set; }
}
