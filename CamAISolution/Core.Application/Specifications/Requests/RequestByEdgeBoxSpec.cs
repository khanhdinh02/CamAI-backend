using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class RequestByEdgeBoxSpec : Specification<Request>
{
    private readonly Guid? id;

    public RequestByEdgeBoxSpec(Guid? id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Request, bool>> GetExpression() => r => r.EdgeBoxId == id;
}
