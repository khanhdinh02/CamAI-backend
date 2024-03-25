using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByStatusSpec : Specification<EdgeBoxInstall>
{
    private readonly EdgeBoxInstallStatus status;

    public EdgeBoxInstallByStatusSpec(EdgeBoxInstallStatus status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression()
    {
        return s => s.EdgeBoxInstallStatus == status;
    }
}
