using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class ShopByStatusSpec : Specification<Shop>
{
    private readonly int status;

    public ShopByStatusSpec(int status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.ShopStatus.Id == status;
    }
}
