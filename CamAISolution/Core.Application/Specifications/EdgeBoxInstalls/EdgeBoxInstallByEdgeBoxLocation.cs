using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByEdgeBoxLocation : Specification<EdgeBoxInstall>
{
    private readonly EdgeBoxLocation location;

    public EdgeBoxInstallByEdgeBoxLocation(EdgeBoxLocation location)
    {
        this.location = location;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression() =>
        ei => ei.EdgeBox.EdgeBoxLocation == location;
}
