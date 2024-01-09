using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class EdgeBoxSearchSpec : RepositorySpec<EdgeBox>
{
    private static Expression<Func<EdgeBox, bool>> GetExpression(SearchEdgeBoxRequest searchRequest)
    {
        var baseSpec = new Specification<EdgeBox>();
        if (!string.IsNullOrEmpty(searchRequest.Model))
            baseSpec.And(new EdgeBoxByModelSpec(searchRequest.Model));

        if (searchRequest.EdgeBoxStatusId.HasValue)
            baseSpec.And(new EdgeBoxByStatusSpec(searchRequest.EdgeBoxStatusId.Value));

        if (searchRequest.EdgeBoxLocationId.HasValue)
            baseSpec.And(new EdgeBoxByLocationSpec(searchRequest.EdgeBoxLocationId.Value));
        return baseSpec.GetExpression();
    }

    public EdgeBoxSearchSpec(SearchEdgeBoxRequest searchRequest)
        : base(GetExpression(searchRequest))
    {
        ApplyingPaging(searchRequest);
        ApplyOrderByDescending(s => s.CreatedDate);
    }
}
