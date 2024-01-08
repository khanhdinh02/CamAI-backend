using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByBrandSpec : Specification<Account>
{
    private readonly Guid _brandId;

    public AccountByBrandSpec(Guid brandId)
    {
        _brandId = brandId;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() =>
        a =>
            (a.Brand != null && a.Brand.Id == _brandId)
            || (a.ManagingShop != null && a.ManagingShop.BrandId == _brandId)
            || (a.WorkingShop != null && a.WorkingShop.BrandId == _brandId);
}
