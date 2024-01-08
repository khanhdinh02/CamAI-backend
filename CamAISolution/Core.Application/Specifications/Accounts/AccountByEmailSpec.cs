using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByEmailSpec : Specification<Account>
{
    private readonly string _email;

    public AccountByEmailSpec(string email)
    {
        _email = email;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Email.Contains(_email);
}
