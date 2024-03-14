using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class RequestByBrandSpec : Specification<Request>
{
    private readonly Guid id;

    public RequestByBrandSpec(Guid id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Request, bool>> GetExpression() => r => r.Shop!.BrandId == id;
}
