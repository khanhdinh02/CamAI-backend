using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class BrandByStatusSpec : Specification<Brand>
{
    private readonly int brandStatus;

    public BrandByStatusSpec(int brandStatus)
    {
        this.brandStatus = brandStatus;
        Expr = GetExpression();
    }

    public override Expression<Func<Brand, bool>> GetExpression() => x => x.BrandStatusId == brandStatus;
}
