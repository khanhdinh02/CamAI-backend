using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class ShopByIdRepoSpec : EntityByIdSpec<Shop, Guid>
{
    public ShopByIdRepoSpec(Guid id, bool includeAll = true)
        : base(s => s.Id == id)
    {
        if (includeAll)
        {
            AddIncludes(s => s.Ward.District.Province);
            AddIncludes(s => s.Brand);
            AddIncludes(s => s.ShopManager!);
        }
    }
}
