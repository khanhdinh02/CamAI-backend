using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class EdgeBoxByIdRepoSpec : EntityByIdSpec<EdgeBox, Guid>
{
    public EdgeBoxByIdRepoSpec(Guid id)
        : base(x => x.Id == id)
    {
        AddIncludes(x => x.EdgeBoxLocation, x => x.EdgeBoxStatus);
    }
}
