using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class PersonalRequestSearchSpec : RepositorySpec<Request>
{
    public PersonalRequestSearchSpec(SearchPersonalRequestRequest req, Guid accountId)
        : base(GetExpression(req, accountId))
    {
        base.ApplyingPaging(req);
        base.ApplyOrderByDescending(r => r.CreatedDate);
        base.AddIncludes(r => r.Account);
    }

    private static Expression<Func<Request, bool>> GetExpression(SearchPersonalRequestRequest req, Guid accountId)
    {
        var baseSpec = new Specification<Request>();

        if (req.Type.HasValue)
            baseSpec.And(new RequestByTypeSpec(req.Type.Value));

        baseSpec.And(new RequestByAccountSpec(accountId));

        baseSpec.And(new RequestByShopSpec(null));

        baseSpec.And(new RequestByEdgeBoxSpec(null));

        if (req.HasReply.HasValue)
            baseSpec.And(new RequestByReplySpec(req.HasReply.Value));

        if (req.Status.HasValue)
            baseSpec.And(new RequestByStatusSpec(req.Status.Value));

        return baseSpec.GetExpression();
    }
}
