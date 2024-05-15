using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class IncidentByTypeSpec : Specification<Incident>
{
    private readonly IncidentType[] types;

    public IncidentByTypeSpec(IncidentType[] types)
    {
        this.types = types;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return s => types.Contains(s.IncidentType);
    }
}
