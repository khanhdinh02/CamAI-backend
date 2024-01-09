using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class TicketSearchSpec : RepositorySpec<Ticket>
{
    private static Expression<Func<Ticket, bool>> GetExpression(TicketSearchRequest searchReq)
    {
        var baseSpec = new Specification<Ticket>();
        if(searchReq.TicketStatusId.HasValue)
            baseSpec.And(new TicketByStatusIdSpec(searchReq.TicketStatusId.Value));
        if(searchReq.AssignedToId.HasValue)
            baseSpec.And(new TicketByAssignedToIdSpec(searchReq.AssignedToId.Value));
        return baseSpec.GetExpression();
    }

    public TicketSearchSpec(TicketSearchRequest searchReq) : base(GetExpression(searchReq))
    {
        AddIncludes(nameof(Ticket.AssignedTo));
        AddIncludes(nameof(Ticket.TicketStatus));
        ApplyOrderByDescending(s => s.CreatedDate);
        ApplyingPaging(searchReq);
    }
}
