using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class TicketByStatusIdSpec : Specification<Ticket>
{
    private readonly int statusId;
    public TicketByStatusIdSpec(int statusId)
    {
        this.statusId = statusId;
        Expr = GetExpression();
    }

    public override Expression<Func<Ticket, bool>> GetExpression()
    {
        return t => t.TicketStatusId == statusId;
    }
}
