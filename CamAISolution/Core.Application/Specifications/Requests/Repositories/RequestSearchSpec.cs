using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class RequestSearchSpec : RepositorySpec<Request>
{
    public RequestSearchSpec(SearchRequestRequest req)
        : base(GetExpression(req))
    {
        base.ApplyingPaging(req);
        base.ApplyOrderByDescending(r => r.CreatedDate);
        base.AddIncludes(r => r.Account);
        base.AddIncludes(r => r.Shop);
        base.AddIncludes(r => r.EdgeBox);
    }

    private static Expression<Func<Request, bool>> GetExpression(SearchRequestRequest req)
    {
        var baseSpec = new Specification<Request>();

        if (req.Type.HasValue)
            baseSpec.And(new RequestByTypeSpec(req.Type.Value));

        if (req.AccountId.HasValue)
            baseSpec.And(new RequestByAccountSpec(req.AccountId.Value));

        if (req.BrandId.HasValue)
            baseSpec.And(new RequestByBrandSpec(req.BrandId.Value));

        if (req.ShopId.HasValue)
            baseSpec.And(new RequestByShopSpec(req.ShopId.Value));

        if (req.EdgeBoxId.HasValue)
            baseSpec.And(new RequestByEdgeBoxSpec(req.EdgeBoxId.Value));

        if (req.HasReply.HasValue)
            baseSpec.And(new RequestByReplySpec(req.HasReply.Value));

        if (req.Status.HasValue)
            baseSpec.And(new RequestByStatusSpec(req.Status.Value));

        return baseSpec.GetExpression();
    }
}
