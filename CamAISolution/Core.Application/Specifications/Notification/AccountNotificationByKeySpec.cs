using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountNotificationByKeySpec : Specification<AccountNotification>
{
    private readonly Guid accountId;
    private readonly Guid notificationId;

    public AccountNotificationByKeySpec(Guid accountId, Guid notificationId)
    {
        this.accountId = accountId;
        this.notificationId = notificationId;
        Expr = GetExpression();
    }

    public override Expression<Func<AccountNotification, bool>> GetExpression()
    {
        return a => a.NotificationId == notificationId && a.AccountId == accountId;
    }
}
