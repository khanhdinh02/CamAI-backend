using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByBrandSpec : Specification<Account>
{
    private readonly Guid brandId;

    public AccountByBrandSpec(Guid brandId)
    {
        this.brandId = brandId;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() =>
        a => a.Brand!.Id == brandId || a.ManagingShop!.BrandId == brandId || a.WorkingShop!.BrandId == brandId;
}
