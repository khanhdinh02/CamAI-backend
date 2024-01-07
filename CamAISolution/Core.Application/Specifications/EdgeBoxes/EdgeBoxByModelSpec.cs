using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxes;

public class EdgeBoxByModelSpec : Specification<EdgeBox>
{
    private readonly string model;

    public EdgeBoxByModelSpec(string model)
    {
        this.model = model;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBox, bool>> GetExpression() =>
        x => string.Equals(x.Model, model, StringComparison.OrdinalIgnoreCase);
}
