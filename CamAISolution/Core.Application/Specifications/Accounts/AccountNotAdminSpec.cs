using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class AccountNotAdminSpec : Specification<Account>
{
    public AccountNotAdminSpec()
    {
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression()
    {
        return a => a.Role != Role.Admin && a.Role != Role.SystemHandler;
    }
}
