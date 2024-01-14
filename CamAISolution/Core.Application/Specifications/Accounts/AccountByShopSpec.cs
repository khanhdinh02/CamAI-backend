using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByShopSpec : Specification<Account>
{
    private readonly Guid shopId;

    public AccountByShopSpec(Guid shopId)
    {
        this.shopId = shopId;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() =>
        a =>
            a.WorkingShopId == shopId
            || a.ManagingShop!.Id == shopId
            || a.Brand!.Shops.Contains(new Shop { Id = shopId });
}
