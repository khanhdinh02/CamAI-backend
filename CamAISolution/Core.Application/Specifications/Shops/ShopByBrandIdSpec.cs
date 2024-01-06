using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;
public class ShopByBrandIdSpec : Specification<Shop>
{
    private readonly Guid brandId;
    public ShopByBrandIdSpec(Guid brandId)
    {
        this.brandId = brandId;
        Expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.BrandId == brandId;
    }
}
