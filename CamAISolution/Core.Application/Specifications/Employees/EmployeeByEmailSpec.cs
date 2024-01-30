using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class EmployeeByEmailSpec : Specification<Employee>
{
    private readonly string email;

    public EmployeeByEmailSpec(string email)
    {
        this.email = email;
        Expr = GetExpression();
    }

    public override Expression<Func<Employee, bool>> GetExpression() => e => e.Email.Contains(email);
}
