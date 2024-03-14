using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class RequestByShopSpec : Specification<Request>
{
    private readonly Guid? id;

    public RequestByShopSpec(Guid? id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Request, bool>> GetExpression() => r => r.ShopId == id;
}
