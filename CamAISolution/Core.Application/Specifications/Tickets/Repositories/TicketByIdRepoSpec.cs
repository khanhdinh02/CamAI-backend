using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class TicketByIdRepoSpec : EntityByIdSpec<Ticket, Guid>
{
    public TicketByIdRepoSpec(Guid id)
        : base(t => t.Id == id)
    {
        AddIncludes(nameof(Ticket.AssignedTo));
        AddIncludes(nameof(Ticket.Shop));
        AddIncludes(nameof(Ticket.TicketStatus));
        AddIncludes(nameof(Ticket.TicketType));
    }
}
