using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class SearchPersonalRequestRequest : BaseSearchRequest
{
    public RequestType? Type { get; set; }
    public bool? HasReply { get; set; }
    public RequestStatus? Status { get; set; }
}
