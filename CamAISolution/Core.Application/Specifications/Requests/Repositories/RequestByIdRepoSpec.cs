using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class RequestByIdRepoSpec : EntityByIdSpec<Request, Guid>
{
    public RequestByIdRepoSpec(Guid id)
        : base(r => r.Id == id)
    {
        base.AddIncludes(r => r.Account);
        base.AddIncludes(r => r.Shop);
        base.AddIncludes(r => r.EdgeBox);
    }
}
