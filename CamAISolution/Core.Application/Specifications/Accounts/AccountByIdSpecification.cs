using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Accounts;
public class AccountByIdSpecification : Specification<Account>
{
    private readonly Guid id;
    public AccountByIdSpecification(Guid id)
    {
        this.id = id;
        expr = GetExpression();
    }
    public override Expression<Func<Account, bool>> GetExpression()
    {
        return a => a.Id == id;
    }
}
