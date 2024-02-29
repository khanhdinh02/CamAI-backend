using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class EdgeBoxByStatusSpec : Specification<EdgeBox>
{
    private readonly EdgeBoxStatus status;

    public EdgeBoxByStatusSpec(EdgeBoxStatus edgeBoxStatus)
    {
        status = edgeBoxStatus;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBox, bool>> GetExpression() => x => x.EdgeBoxStatus == status;
}
