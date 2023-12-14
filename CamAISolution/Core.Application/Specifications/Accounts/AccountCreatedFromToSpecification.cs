using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Accounts;
public class AccountCreatedFromToSpecification (DateTime from, DateTime to) : Specification<Account>
{
    public override Expression<Func<Account, bool>> ToExpression()
    {
        if (from >= to)
            return a => false;
        return a => a.CreatedDate > from && a.CreatedDate < to;
    }
}
