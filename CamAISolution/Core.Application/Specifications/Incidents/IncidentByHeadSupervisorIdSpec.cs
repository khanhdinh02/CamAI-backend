using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByHeadSupervisorIdSpec : Specification<Incident>
{
    private readonly Guid headSupervisorId;

    public IncidentByHeadSupervisorIdSpec(Guid headSupervisorId)
    {
        this.headSupervisorId = headSupervisorId;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return s => s.HeadSupervisorId == headSupervisorId;
    }
}
