using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class EmployeeByStatusSpec : Specification<Employee>
{
    private readonly int statusId;

    public EmployeeByStatusSpec(int statusId)
    {
        this.statusId = statusId;
        Expr = GetExpression();
    }

    public override Expression<Func<Employee, bool>> GetExpression() => e => e.EmployeeStatusId == statusId;
}
