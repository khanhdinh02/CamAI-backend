using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class BrandByNameSpec : Specification<Brand>
{
    private readonly string name;

    public BrandByNameSpec(string name)
    {
        this.name = name;
        Expr = GetExpression();
    }

    public override Expression<Func<Brand, bool>> GetExpression() =>
        x => x.Name.Trim().ToLower().Contains(name.Trim().ToLower());
}
