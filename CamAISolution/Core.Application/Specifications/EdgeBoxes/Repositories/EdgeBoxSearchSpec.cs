using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class EdgeBoxSearchSpec : RepositorySpec<EdgeBox>
{
    private static Expression<Func<EdgeBox, bool>> GetExpression(SearchEdgeBoxRequest searchRequest)
    {
        var baseSpec = new Specification<EdgeBox>();
        if (!string.IsNullOrEmpty(searchRequest.Name))
            baseSpec.And(new EdgeBoxByNameSpec(searchRequest.Name));

        if (searchRequest.EdgeBoxStatus.HasValue)
            baseSpec.And(new EdgeBoxByStatusSpec(searchRequest.EdgeBoxStatus.Value));

        if (searchRequest.EdgeBoxLocation.HasValue)
            baseSpec.And(new EdgeBoxByLocationSpec(searchRequest.EdgeBoxLocation.Value));
        return baseSpec.GetExpression();
    }

    public EdgeBoxSearchSpec(SearchEdgeBoxRequest searchRequest)
        : base(GetExpression(searchRequest))
    {
        ApplyingPaging(searchRequest);
        ApplyOrderByDescending(s => s.CreatedDate);
        AddIncludes(eb => eb.EdgeBoxModel);
    }
}
