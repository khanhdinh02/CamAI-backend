using System.Linq.Expressions;
using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;
using Core.Domain.Models.DTO.Shops;

namespace Core.Application.Specifications.Shops.Repositories;

public class SearchShopSpec : RepositorySpec<Shop>
{
    private static Expression<Func<Shop, bool>> GetExpression(SearchShopRequest search)
    {
        var baseSpec = new Specification<Shop>();
        if (!string.IsNullOrEmpty(search.Name))
            baseSpec.And(new ShopByNameSpec(search.Name));
        if (search.StatusId.HasValue)
            baseSpec.And(new ShopByStatusSpec(search.StatusId.Value));
        return baseSpec.GetExpression();
    }

    public SearchShopSpec(SearchShopRequest search)
        : base(GetExpression(search))
    {
        AddIncludes(s => s.ShopStatus);
        ApplyingPaging(search);
        ApplyOrderByDescending(s => s.CreatedDate);
    }
}
