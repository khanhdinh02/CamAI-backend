using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class RequestByReplySpec : Specification<Request>
{
    private readonly bool hasReply;

    public RequestByReplySpec(bool hasReply)
    {
        this.hasReply = hasReply;
        Expr = GetExpression();
    }

    public override Expression<Func<Request, bool>> GetExpression() => r => (r.Reply != null) == hasReply;
}
