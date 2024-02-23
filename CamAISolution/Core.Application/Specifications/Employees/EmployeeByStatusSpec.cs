using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications;

public class EmployeeByStatusSpec : Specification<Employee>
{
    private readonly EmployeeStatus status;

    public EmployeeByStatusSpec(EmployeeStatus status)
    {
        this.status = status;
        Expr = GetExpression();
    }

    public override Expression<Func<Employee, bool>> GetExpression() => e => e.EmployeeStatus == status;
}
