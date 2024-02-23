using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchAccountRequest : BaseSearchRequest
{
    /// <summary>
    /// Name, Email, or Phone
    /// </summary>
    public string? Name { get; set; }
    public string? Email { get; set; }
    public AccountStatus? AccountStatus { get; set; }
    public Role? Role { get; set; }

    /// <summary>
    /// The result includes Brand manager, Shop managers, and Employees
    /// </summary>
    public Guid? BrandId { get; set; }
}
