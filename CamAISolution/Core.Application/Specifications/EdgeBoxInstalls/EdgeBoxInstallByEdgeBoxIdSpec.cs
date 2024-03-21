using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByEdgeBoxIdSpec : Specification<EdgeBoxInstall>
{
    private readonly Guid edgeBoxId;

    public EdgeBoxInstallByEdgeBoxIdSpec(Guid edgeBoxId)
    {
        this.edgeBoxId = edgeBoxId;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression()
    {
        return s => s.EdgeBoxId == edgeBoxId;
    }
}
