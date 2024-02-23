using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class AccountByStatusSpec : Specification<Account>
{
    private readonly AccountStatus status;

    public AccountByStatusSpec(AccountStatus status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.AccountStatus == status;
}
