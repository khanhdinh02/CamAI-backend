using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class ShopByStatusSpec : Specification<Shop>
{
    private readonly ShopStatus status;

    public ShopByStatusSpec(ShopStatus status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.ShopStatus == status;
    }
}
