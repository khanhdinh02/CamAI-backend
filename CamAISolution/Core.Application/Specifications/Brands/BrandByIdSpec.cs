using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class BrandByIdSpec : Specification<Brand>
{
    private readonly Guid id;

    public BrandByIdSpec(Guid id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Brand, bool>> GetExpression()
    {
        return brand => brand.Id == id;
    }
}
