using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class ShopByNameSpec : Specification<Shop>
{
    private readonly string name;

    public ShopByNameSpec(string name)
    {
        this.name = name;
        Expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.Name.Trim().ToLower().Contains(name.Trim().ToLower());
    }
}
