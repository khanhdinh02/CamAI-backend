using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByActivationStatus : Specification<EdgeBoxInstall>
{
    private readonly EdgeBoxActivationStatus status;

    public EdgeBoxInstallByActivationStatus(EdgeBoxActivationStatus status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression() => ei => ei.ActivationStatus == status;
}
