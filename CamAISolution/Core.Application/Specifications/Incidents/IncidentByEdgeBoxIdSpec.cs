using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByEdgeBoxIdSpec : Specification<Incident>
{
    private readonly Guid edgeBoxId;

    public IncidentByEdgeBoxIdSpec(Guid edgeBoxId)
    {
        this.edgeBoxId = edgeBoxId;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return s => s.EdgeBoxId == edgeBoxId;
    }
}
