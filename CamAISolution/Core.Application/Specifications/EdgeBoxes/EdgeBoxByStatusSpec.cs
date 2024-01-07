using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxes;

public class EdgeBoxByStatusSpec : Specification<EdgeBox>
{
    private readonly int statusId;

    public EdgeBoxByStatusSpec(int edgeBoxStatusId)
    {
        statusId = edgeBoxStatusId;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBox, bool>> GetExpression() => x => x.EdgeBoxStatusId == statusId;
}
