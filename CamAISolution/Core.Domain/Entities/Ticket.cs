using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class Ticket : BusinessEntity
{
    public int TicketTypeId { get; set; }
    public Guid? AssignedToId { get; set; }
    public Guid? ShopId { get; set; }
    public string? Detail { get; set; }

    /// <summary>
    /// Reply from technician
    /// </summary>
    public string? Reply { get; set; }
    public int TicketStatusId { get; set; }

    public virtual TicketType TicketType { get; set; } = null!;
    public virtual Account? AssignedTo { get; set; }
    public virtual Shop? Shop { get; set; }
    public virtual TicketStatus TicketStatus { get; set; } = null!;
}
