using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByShopId : Specification<EdgeBoxInstall>
{
    private readonly Guid shopId;

    public EdgeBoxInstallByShopId(Guid shopId)
    {
        this.shopId = shopId;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression() => ei => ei.ShopId == shopId;
}
