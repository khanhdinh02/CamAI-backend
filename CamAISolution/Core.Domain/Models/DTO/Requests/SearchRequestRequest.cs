using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchRequestRequest : BaseSearchRequest
{
    public RequestType? Type { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? ShopId { get; set; }
    public Guid? EdgeBoxId { get; set; }
    public bool? HasReply { get; set; }
    public RequestStatus? Status { get; set; }
}
