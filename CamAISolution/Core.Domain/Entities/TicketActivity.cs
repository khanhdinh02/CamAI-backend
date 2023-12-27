using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class TicketActivity : ActivityEntity
{
    public Guid? TicketId { get; set; }
    public int? OldStatusId { get; set; }
    public int? NewStatusId { get; set; }

    public virtual Ticket? Ticket { get; set; } = null!;
    public virtual TicketStatus? OldStatus { get; set; } = null!;
    public virtual TicketStatus? NewStatus { get; set; } = null!;
}
