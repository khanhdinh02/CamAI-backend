using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class ShopByWardSpec : Specification<Shop>
{
    private readonly int wardId;

    public ShopByWardSpec(int wardId)
    {
        this.wardId = wardId;
        Expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.WardId == wardId;
    }
}
