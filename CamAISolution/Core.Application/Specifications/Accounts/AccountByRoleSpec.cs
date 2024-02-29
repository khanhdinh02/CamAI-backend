using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class AccountByRoleSpec : Specification<Account>
{
    private readonly Role role;

    public AccountByRoleSpec(Role role)
    {
        this.role = role;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Role == role;
}
