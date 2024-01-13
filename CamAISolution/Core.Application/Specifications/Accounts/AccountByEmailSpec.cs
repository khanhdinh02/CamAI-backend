using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByEmailSpec : Specification<Account>
{
    private readonly string email;

    public AccountByEmailSpec(string email)
    {
        this.email = email;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Email.Contains(email);
}
