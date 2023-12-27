using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Brands.Repositories;

public class BrandByIdRepoSpec : EntityByIdSpec<Brand, Guid>
{
    public BrandByIdRepoSpec(Guid id)
        : base(x => x.Id == id)
    {
        AddIncludes(x => x.BrandStatus);
    }
}
