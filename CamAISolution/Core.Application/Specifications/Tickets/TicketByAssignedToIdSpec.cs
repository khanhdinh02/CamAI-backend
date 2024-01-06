using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class TicketByAssignedToIdSpec : Specification<Ticket>
{
    private readonly Guid assignedToId;
    public TicketByAssignedToIdSpec(Guid assignedToId)
    {
        this.assignedToId = assignedToId;
        Expr = GetExpression();
    }

    public override Expression<Func<Ticket, bool>> GetExpression()
    {
        return t => t.AssignedToId == assignedToId;
    }
}
