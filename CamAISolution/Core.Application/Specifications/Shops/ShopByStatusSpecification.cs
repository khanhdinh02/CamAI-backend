using System.Linq.Expressions;
using Core.Application.Specifications;
using Core.Domain.Entities;

namespace Core.Application;

public class ShopByStatusSpecification : Specification<Shop>
{
    private readonly Guid status;

    public ShopByStatusSpecification(Guid status)
    {
        this.status = status;
        expr = GetExpression();
    }
    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.ShopStatus.Id == status;
    }
}
