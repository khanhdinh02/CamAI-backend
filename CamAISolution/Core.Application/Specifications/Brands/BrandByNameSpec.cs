using System.Linq.Expressions;
using Core.Application.Specifications;
using Core.Domain.Entities;

namespace Core.Application;

public class BrandByNameSpec : Specification<Brand>
{
    private readonly string name;

    public BrandByNameSpec(string name)
    {
        this.name = name;
        expr = GetExpression();
    }

    public override Expression<Func<Brand, bool>> GetExpression() =>
        x => string.Equals(x.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase);
}
