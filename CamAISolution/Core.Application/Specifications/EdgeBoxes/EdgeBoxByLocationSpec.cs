using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class EdgeBoxByLocationSpec : Specification<EdgeBox>
{
    private readonly EdgeBoxLocation edgeBoxLocation;

    public EdgeBoxByLocationSpec(EdgeBoxLocation edgeBoxLocation)
    {
        this.edgeBoxLocation = edgeBoxLocation;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBox, bool>> GetExpression() => x => x.EdgeBoxLocation == edgeBoxLocation;
}
