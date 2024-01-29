using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class ShopByPhoneSpec : Specification<Shop>
{
    private readonly string phone;

    public ShopByPhoneSpec(string phone)
    {
        this.phone = phone;
        Expr = GetExpression();
    }

    public override Expression<Func<Shop, bool>> GetExpression()
    {
        return s => s.Phone!.Trim().ToLower().Contains(phone.Trim().ToLower());
    }
}
