using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByStartLastSeen : Specification<EdgeBoxInstall>
{
    private readonly DateTime startLastSeen;

    public EdgeBoxInstallByStartLastSeen(DateTime startLastSeen)
    {
        this.startLastSeen = startLastSeen;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression() => ei => ei.LastSeen >= startLastSeen;
}
