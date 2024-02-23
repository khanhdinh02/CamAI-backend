using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class BrandByStatusSpec : Specification<Brand>
{
    private readonly BrandStatus brandStatus;

    public BrandByStatusSpec(BrandStatus brandStatus)
    {
        this.brandStatus = brandStatus;
        Expr = GetExpression();
    }

    public override Expression<Func<Brand, bool>> GetExpression() => x => x.BrandStatus == brandStatus;
}
