using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByStatusSpec : Specification<Account>
{
    private readonly int _statusId;

    public AccountByStatusSpec(int statusId)
    {
        _statusId = statusId;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.AccountStatusId == _statusId;
}
