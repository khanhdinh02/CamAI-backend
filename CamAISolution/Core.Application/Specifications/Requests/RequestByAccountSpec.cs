using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class RequestByAccountSpec : Specification<Request>
{
    private readonly Guid id;

    public RequestByAccountSpec(Guid id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Request, bool>> GetExpression() => r => r.AccountId == id;
}
