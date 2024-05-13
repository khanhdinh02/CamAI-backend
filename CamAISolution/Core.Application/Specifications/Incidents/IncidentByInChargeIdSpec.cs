using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByInChargeIdSpec : Specification<Incident>
{
    private readonly Guid inChargeId;

    public IncidentByInChargeIdSpec(Guid inChargeId)
    {
        this.inChargeId = inChargeId;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return i => i.InChargeAccountId == inChargeId || i.HeadSupervisorId == inChargeId;
    }
}
