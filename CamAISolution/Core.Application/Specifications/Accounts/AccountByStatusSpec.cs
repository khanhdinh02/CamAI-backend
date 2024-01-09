using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByStatusSpec : Specification<Account>
{
    private readonly int statusId;

    public AccountByStatusSpec(int statusId)
    {
        this.statusId = statusId;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.AccountStatusId == statusId;
}
