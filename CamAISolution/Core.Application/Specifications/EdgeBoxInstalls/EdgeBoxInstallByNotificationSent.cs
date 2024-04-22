using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxInstalls;

public class EdgeBoxInstallByNotificationSent : Specification<EdgeBoxInstall>
{
    private readonly bool notificationSent;

    public EdgeBoxInstallByNotificationSent(bool notificationSent)
    {
        this.notificationSent = notificationSent;
        Expr = GetExpression();
    }

    public override Expression<Func<EdgeBoxInstall, bool>> GetExpression() =>
        ei => ei.NotificationSent == notificationSent;
}
