using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByEndLastSeen : Specification<EdgeBoxInstall>
{
    private readonly DateTime endLastSeen;

    public EdgeBoxInstallByEndLastSeen(DateTime endLastSeen)
    {
        this.endLastSeen = endLastSeen;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression() => ei => ei.LastSeen <= endLastSeen;
}
