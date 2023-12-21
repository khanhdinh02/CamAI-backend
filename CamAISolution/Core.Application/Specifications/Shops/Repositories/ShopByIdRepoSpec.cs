using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class ShopByIdRepoSpec : EntityByIdSpec<Shop>
{
    public ShopByIdRepoSpec(Guid id, bool includeAll = true)
        : base(s => s.Id == id)
    {
        if (includeAll)
        {
            //Include all data of address
            AddIncludes(s => s.Ward.District.Province);
        }
    }
}
