using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByFromTimeSpec : Specification<Incident>
{
    private readonly DateTime fromTime;

    public IncidentByFromTimeSpec(DateTime fromTime)
    {
        this.fromTime = fromTime;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return x => x.StartTime >= fromTime;
    }
}
