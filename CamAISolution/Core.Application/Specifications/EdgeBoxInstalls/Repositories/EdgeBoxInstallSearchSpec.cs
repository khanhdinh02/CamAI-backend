using System.Linq.Expressions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.EdgeBoxInstalls.Repositories;

public class EdgeBoxInstallSearchSpec : RepositorySpec<EdgeBoxInstall>
{
    private static Expression<Func<EdgeBoxInstall, bool>> GetExpression(SearchEdgeBoxInstallRequest search)
    {
        var baseSpec = new Specification<EdgeBoxInstall>();
        if (search.EdgeBoxId.HasValue)
            baseSpec.And(new EdgeBoxInstallByEdgeBoxIdSpec(search.EdgeBoxId.Value));
        if (search.EdgeBoxLocation.HasValue)
            baseSpec.And(new EdgeBoxInstallByEdgeBoxLocation(search.EdgeBoxLocation.Value));
        if (search.EdgeBoxInstallStatus.HasValue)
            baseSpec.And(new EdgeBoxInstallByStatusSpec(search.EdgeBoxInstallStatus.Value));
        if (search.ShopId.HasValue)
            baseSpec.And(new EdgeBoxInstallByShopId(search.ShopId.Value));
        if (search.ActivationStatus.HasValue)
            baseSpec.And(new EdgeBoxInstallByActivationStatus(search.ActivationStatus.Value));
        if (search.StartLastSeen.HasValue)
            baseSpec.And(new EdgeBoxInstallByStartLastSeen(search.StartLastSeen.Value));
        if (search.EndLastSeen.HasValue)
            baseSpec.And(new EdgeBoxInstallByEndLastSeen(search.EndLastSeen.Value));

        return baseSpec.GetExpression();
    }

    public EdgeBoxInstallSearchSpec(SearchEdgeBoxInstallRequest search)
        : base(GetExpression(search))
    {
        ApplyingPaging(search);
        AddIncludes(x => x.Shop);
        AddIncludes(x => x.EdgeBox);
        ApplyOrderByDescending(s => s.CreatedDate);
    }
}
