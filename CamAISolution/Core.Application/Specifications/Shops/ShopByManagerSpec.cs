using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class ShopByManagerSpec : Specification<Shop>
{
    private readonly Guid shopManagerId;
    public ShopByManagerSpec(Guid shopManagerId)
    {
        this.shopManagerId = shopManagerId;
        Expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.ShopManagerId == shopManagerId;
    }
}
