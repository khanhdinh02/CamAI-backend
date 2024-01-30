using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class EmployeeByShopSpec : Specification<Employee>
{
    private readonly Guid shopId;

    public EmployeeByShopSpec(Guid shopId)
    {
        this.shopId = shopId;
        Expr = GetExpression();
    }

    public override Expression<Func<Employee, bool>> GetExpression() => e => e.ShopId == shopId;
}
