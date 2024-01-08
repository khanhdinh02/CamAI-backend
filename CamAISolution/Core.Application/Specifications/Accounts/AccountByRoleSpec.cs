using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByRoleSpec : Specification<Account>
{
    private readonly int _roleId;

    public AccountByRoleSpec(int roleId)
    {
        _roleId = roleId;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Roles.Contains(new Role { Id = _roleId });
}
