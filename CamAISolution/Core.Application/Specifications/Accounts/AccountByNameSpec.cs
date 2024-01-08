using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByNameSpec : Specification<Account>
{
    private readonly string _name;

    public AccountByNameSpec(string name)
    {
        _name = name;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Name.Contains(_name);
}
