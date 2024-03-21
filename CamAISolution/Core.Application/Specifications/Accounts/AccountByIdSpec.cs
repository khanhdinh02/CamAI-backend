using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class AccountByIdSpec : Specification<Account>
{
    private readonly Guid id;

    public AccountByIdSpec(Guid id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression()
    {
        return a => a.Id == id && a.Role != Role.Admin;
    }
}
