using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class BrandByIdRepoSpec : EntityByIdSpec<Brand>
{
    public BrandByIdRepoSpec(Guid id)
        : base(x => x.Id == id)
    {
        AddIncludes(x => x.BrandStatus);
    }
}
