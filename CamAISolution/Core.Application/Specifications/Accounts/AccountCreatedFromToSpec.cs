using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountCreatedFromToSpec : Specification<Account>
{
    private readonly DateTime from;
    private readonly DateTime to;

    public AccountCreatedFromToSpec(DateTime from, DateTime to)
    {
        this.from = from;
        this.to = to;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression()
    {
        if (from >= to)
            return a => false;
        return a => a.CreatedDate >= from && a.CreatedDate <= to;
    }
}
