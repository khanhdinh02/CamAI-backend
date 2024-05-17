using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByToTimeSpec : Specification<Incident>
{
    private readonly DateTime toTime;

    public IncidentByToTimeSpec(DateTime toTime)
    {
        this.toTime = toTime;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return x => x.StartTime <= toTime;
    }
}
