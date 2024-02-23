using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class EmployeeByIdRepoSpec : EntityByIdSpec<Employee, Guid>
{
    public EmployeeByIdRepoSpec(Guid id)
        : base(e => e.Id == id)
    {
        AddIncludes(e => e.Shop);
        AddIncludes(e => e.Ward!.District.Province);
    }
}
