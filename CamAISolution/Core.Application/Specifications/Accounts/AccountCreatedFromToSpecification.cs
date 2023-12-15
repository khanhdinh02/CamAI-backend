using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Accounts;
public class AccountCreatedFromToSpecification : Specification<Account>
{
    private readonly DateTime from;
    private readonly DateTime to;
    public AccountCreatedFromToSpecification(DateTime from, DateTime to)
    {
        this.from = from;
        this.to = to;
        expr = GetExpression();
    }
    public override Expression<Func<Account, bool>> GetExpression()
    {
        if (from >= to)
            return a => false;
        return a => a.CreatedDate >= from && a.CreatedDate <= to;
    }
}
