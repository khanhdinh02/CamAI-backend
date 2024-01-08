using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByShopSpec : Specification<Account>
{
    private readonly Guid _shopId;

    public AccountByShopSpec(Guid shopId)
    {
        _shopId = shopId;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() =>
        a =>
            a.WorkingShopId == _shopId
            || (a.ManagingShop != null && a.ManagingShop.Id == _shopId)
            || (a.Brand != null && a.Brand.Shops.Contains(new Shop { Id = _shopId }));
}
