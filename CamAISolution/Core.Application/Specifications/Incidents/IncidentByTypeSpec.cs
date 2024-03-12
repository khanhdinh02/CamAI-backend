using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class IncidentByTypeSpec : Specification<Incident>
{
    private readonly IncidentType status;

    public IncidentByTypeSpec(IncidentType status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return s => s.IncidentType == status;
    }
}
