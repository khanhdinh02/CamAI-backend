using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class EmployeeSearchSpec : RepositorySpec<Employee>
{
    public EmployeeSearchSpec(SearchEmployeeRequest req)
        : base(GetExpression(req))
    {
        AddIncludes(e => e.Shop);
        AddIncludes(e => e.Ward!.District.Province);
        AddIncludes(e => e.EmployeeStatus);
        ApplyingPaging(req);
    }

    private static Expression<Func<Employee, bool>> GetExpression(SearchEmployeeRequest req)
    {
        var baseSpec = new Specification<Employee>();

        if (!string.IsNullOrWhiteSpace(req.Search))
        {
            req.Search = req.Search.Trim();
            baseSpec.And(new EmployeeByNameSpec(req.Search));
            baseSpec.Or(new EmployeeByEmailSpec(req.Search));
            baseSpec.Or(new EmployeeByPhoneSpec(req.Search));
        }

        if (req.EmployeeStatusId.HasValue)
            baseSpec.And(new EmployeeByStatusSpec(req.EmployeeStatusId.Value));

        if (req.BrandId.HasValue)
            baseSpec.And(new EmployeeByBrandSpec(req.BrandId.Value));

        if (req.ShopId.HasValue)
            baseSpec.And(new EmployeeByShopSpec(req.ShopId.Value));

        return baseSpec.GetExpression();
    }
}
