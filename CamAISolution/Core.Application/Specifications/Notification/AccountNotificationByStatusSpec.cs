using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class AccountNotificationByStatusSpec : Specification<AccountNotification>
{
    private readonly NotificationStatus status;

    public AccountNotificationByStatusSpec(NotificationStatus status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<AccountNotification, bool>> GetExpression()
    {
        return an => an.Status == status;
    }
}
