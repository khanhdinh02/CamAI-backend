using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Accounts;
public class AccountByIdSpecification(Guid id) : Specification<Account>
{
    public override Expression<Func<Account, bool>> ToExpression()
    {
        return a => a.Id == id;
    }
}
