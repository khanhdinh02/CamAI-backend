using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByBrandIdSpec : Specification<Incident>
{
    private readonly Guid brandId;

    public IncidentByBrandIdSpec(Guid brandId)
    {
        this.brandId = brandId;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return s => s.Shop.BrandId == brandId;
    }
}
