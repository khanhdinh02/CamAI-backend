using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class RequestByStatusSpec : Specification<Request>
{
    private readonly RequestStatus status;

    public RequestByStatusSpec(RequestStatus status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<Request, bool>> GetExpression() => r => r.RequestStatus == status;
}
