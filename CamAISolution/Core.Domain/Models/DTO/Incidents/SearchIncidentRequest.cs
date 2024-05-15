using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchIncidentRequest : BaseSearchRequest
{
    // shop, brand, edge box, employee,...
    public IncidentTypeRequestOption? IncidentType { get; set; }
    public DateTime? FromTime { get; set; }
    public DateTime? ToTime { get; set; }
    public Guid? EdgeBoxId { get; set; }
    public IncidentStatus? Status { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? InChargeId { get; set; }
}
