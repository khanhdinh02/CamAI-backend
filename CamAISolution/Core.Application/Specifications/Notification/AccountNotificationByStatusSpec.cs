using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountNotificationByStatusSpec : Specification<AccountNotification>
{
    private readonly int status;

    public AccountNotificationByStatusSpec(int status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<AccountNotification, bool>> GetExpression()
    {
        return an => an.StatusId == status;
    }
}
