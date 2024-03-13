using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class RequestByTypeSpec : Specification<Request>
{
    private readonly RequestType type;

    public RequestByTypeSpec(RequestType type)
    {
        this.type = type;
        Expr = GetExpression();
    }

    public override Expression<Func<Request, bool>> GetExpression() => r => r.RequestType == type;
}
