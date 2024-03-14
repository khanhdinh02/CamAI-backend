using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class IncidentByEmployeeIdSpec : Specification<Incident>
{
    private readonly Guid employeeId;

    public IncidentByEmployeeIdSpec(Guid employeeId)
    {
        this.employeeId = employeeId;
        Expr = GetExpression();
    }

    public override Expression<Func<Incident, bool>> GetExpression()
    {
        return s => s.EmployeeId == employeeId;
    }
}
