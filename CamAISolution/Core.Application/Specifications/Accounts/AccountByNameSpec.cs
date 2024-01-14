using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByNameSpec : Specification<Account>
{
    private readonly string name;

    public AccountByNameSpec(string name)
    {
        this.name = name;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Name.Contains(name);
}
