using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class RequestActivity : ActivityEntity
{
    public Guid? RequestId { get; set; }
    public RequestStatus? OldStatus { get; set; }
    public RequestStatus? NewStatus { get; set; }

    public virtual Request? Request { get; set; } = null!;
}
