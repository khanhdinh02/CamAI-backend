using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class EmployeeByIdRepoSpec : EntityByIdSpec<Employee, Guid>
{
    public EmployeeByIdRepoSpec(Guid id)
        : base(e => e.Id == id)
    {
        AddIncludes(e => e.Shop);
        AddIncludes(e => e.Ward!.District.Province);
        AddIncludes(e => e.EmployeeStatus);
    }
}
