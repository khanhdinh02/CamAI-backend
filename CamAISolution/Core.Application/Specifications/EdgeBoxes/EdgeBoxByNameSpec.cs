using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class EdgeBoxByNameSpec : Specification<EdgeBox>
{
    private readonly string model;

    public EdgeBoxByNameSpec(string model)
    {
        this.model = model;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBox, bool>> GetExpression() => x => x.Name == model;
}
