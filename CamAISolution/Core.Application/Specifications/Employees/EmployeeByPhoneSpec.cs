using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class EmployeeByPhoneSpec : Specification<Employee>
{
    private readonly string phone;

    public EmployeeByPhoneSpec(string phone)
    {
        this.phone = phone;
        Expr = GetExpression();
    }

    public override Expression<Func<Employee, bool>> GetExpression() => e => e.Phone.Contains(phone);
}
