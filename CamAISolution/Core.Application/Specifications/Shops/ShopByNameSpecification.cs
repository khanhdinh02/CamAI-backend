using System.Linq.Expressions;
using Core.Application.Specifications;
using Core.Domain.Entities;

namespace Core.Application;

public class ShopByNameSpecification : Specification<Shop>
{
    private readonly string name;
    public ShopByNameSpecification(string name)
    {
        this.name = name;
        expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.Name.Trim().ToLower().Equals(name.Trim().ToLower());
    }
}
