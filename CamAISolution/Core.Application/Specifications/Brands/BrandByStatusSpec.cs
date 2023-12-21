using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Brands;

public class BrandByStatusSpec : Specification<Brand>
{
    private readonly Guid brandStatus;

    public BrandByStatusSpec(Guid brandStatus)
    {
        this.brandStatus = brandStatus;
        expr = GetExpression();
    }

    public override Expression<Func<Brand, bool>> GetExpression() => x => x.BrandStatusId == brandStatus;
}
