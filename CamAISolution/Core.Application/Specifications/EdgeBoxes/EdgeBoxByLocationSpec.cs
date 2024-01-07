using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxes;

public class EdgeBoxByLocationSpec : Specification<EdgeBox>
{
    private readonly int locationId;

    public EdgeBoxByLocationSpec(int edgeBoxLocationId)
    {
        locationId = edgeBoxLocationId;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBox, bool>> GetExpression() => x => x.EdgeBoxLocationId == locationId;
}
