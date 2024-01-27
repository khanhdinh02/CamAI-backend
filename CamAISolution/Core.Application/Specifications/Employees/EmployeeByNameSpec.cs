using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class EmployeeByNameSpec : Specification<Employee>
{
    private readonly string name;

    public EmployeeByNameSpec(string name)
    {
        this.name = name;
        Expr = GetExpression();
    }

    public override Expression<Func<Employee, bool>> GetExpression() => e => e.Name.Contains(name);
}
