using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class EmployeeByBrandSpec : Specification<Employee>
{
    private readonly Guid brandId;

    public EmployeeByBrandSpec(Guid brandId)
    {
        this.brandId = brandId;
        Expr = GetExpression();
    }

    public override Expression<Func<Employee, bool>> GetExpression() => e => e.Shop.BrandId == brandId;
}
