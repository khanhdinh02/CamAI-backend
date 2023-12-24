using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class RequestActivity : ActivityEntity
{
    public Guid? RequestId { get; set; }
    public int? OldStatusId { get; set; }
    public int? NewStatusId { get; set; }

    public virtual Request? Request { get; set; } = null!;
    public virtual RequestStatus? OldStatus { get; set; } = null!;
    public virtual RequestStatus? NewStatus { get; set; } = null!;
}
