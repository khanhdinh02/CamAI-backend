using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByPhoneSpec : Specification<Account>
{
    private readonly string phone;

    public AccountByPhoneSpec(string phone)
    {
        this.phone = phone;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Phone != null && a.Phone.Contains(phone);
}
