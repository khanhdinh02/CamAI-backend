using System.Linq.Expressions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Incidents.Repositories;

public class IncidentSearchSpec : RepositorySpec<Incident>
{
    private static Expression<Func<Incident, bool>> GetExpression(SearchIncidentRequest search)
    {
        var baseSpec = new Specification<Incident>();

        if (search.Status.HasValue)
            baseSpec.And(new IncidentByStatusSpec(search.Status.Value));

        if (search.IncidentType.HasValue)
            baseSpec.And(new IncidentByTypeSpec(search.IncidentType.Value.ToIncidentTypes()));

        if (search.BrandId.HasValue)
            baseSpec.And(new IncidentByBrandIdSpec(search.BrandId.Value));

        if (search.InChargeId.HasValue)
            baseSpec.And(new IncidentByInChargeIdSpec(search.InChargeId.Value));

        if (search.EdgeBoxId.HasValue)
            baseSpec.And(new IncidentByEdgeBoxIdSpec(search.EdgeBoxId.Value));

        if (search.ShopId.HasValue)
            baseSpec.And(new IncidentByShopIdSpec(search.ShopId.Value));

        if (search.EmployeeId.HasValue)
            baseSpec.And(new IncidentByEmployeeIdSpec(search.EmployeeId.Value));

        if (search.FromTime.HasValue)
            baseSpec.And(new IncidentByFromTimeSpec(search.FromTime.Value));

        if (search.ToTime.HasValue)
            baseSpec.And(new IncidentByToTimeSpec(search.ToTime.Value));

        return baseSpec.GetExpression();
    }

    public IncidentSearchSpec(SearchIncidentRequest search, bool includeShop = false)
        : base(GetExpression(search))
    {
        if (includeShop)
            AddIncludes(x => x.Shop);
        AddIncludes(x => x.Employee);
        AddIncludes(x => x.Evidences, x => x.Assignment);
        ApplyingPaging(search);
        ApplyOrderByDescending(s => s.CreatedDate);
    }
}
