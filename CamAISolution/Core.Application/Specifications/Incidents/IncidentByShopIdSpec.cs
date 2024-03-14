using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByShopIdSpec : Specification<Incident>
{
    private readonly Guid shopId;

    public IncidentByShopIdSpec(Guid shopId)
    {
        this.shopId = shopId;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return s => s.ShopId == shopId;
    }
}
