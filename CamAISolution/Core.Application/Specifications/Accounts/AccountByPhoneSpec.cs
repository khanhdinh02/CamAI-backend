using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class AccountByPhoneSpec : Specification<Account>
{
    private readonly string _phone;

    public AccountByPhoneSpec(string phone)
    {
        _phone = phone;
        Expr = GetExpression();
    }

    public override Expression<Func<Account, bool>> GetExpression() => a => a.Phone != null && a.Phone.Contains(_phone);
}
