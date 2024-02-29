using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class BrandByIdRepoSpec : EntityByIdSpec<Brand, Guid>
{
    public BrandByIdRepoSpec(Guid id)
        : base(x => x.Id == id)
    {
        AddIncludes(b => b.BrandManager!);
        AddIncludes(nameof(Brand.Logo));
        AddIncludes(nameof(Brand.Banner));
    }
}
