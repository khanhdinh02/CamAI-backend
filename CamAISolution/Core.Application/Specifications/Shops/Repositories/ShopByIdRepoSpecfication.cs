using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;

namespace Core.Application;

public class ShopByIdRepoSpecfication : RepositorySpecification<Shop>
{
    public ShopByIdRepoSpecfication(Guid id, bool includeAll = true) : base(s => s.Id == id)
    {
        // if (includeAll)
        // {
        //     //Include all data of address
        //     AddIncludes(s => s.Ward.District.Province);
        // }
        ApplyingPaging(1, 0);
        ApplyOrderBy(s => s.Id);
    }
}
