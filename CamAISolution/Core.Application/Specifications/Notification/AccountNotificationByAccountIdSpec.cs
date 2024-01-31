using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountNotificationByAccountIdSpec : Specification<AccountNotification>
{
    private readonly Guid id;

    public AccountNotificationByAccountIdSpec(Guid id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<AccountNotification, bool>> GetExpression()
    {
        return an => an.AccountId == id;
    }
}
