using System.Linq.Expressions;
using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Shops.Repositories;

public class SearchShopSpec : RepositorySpec<Shop>
{
    private static Expression<Func<Shop, bool>> GetExpression(ShopSearchRequest search)
    {
        var baseSpec = new Specification<Shop>();
        if (!string.IsNullOrEmpty(search.Name))
            baseSpec.And(new ShopByNameSpec(search.Name));

        if (search.StatusId.HasValue)
            baseSpec.And(new ShopByStatusSpec(search.StatusId.Value));
        else
            baseSpec.And(new ShopByStatusSpec(ShopStatusEnum.Active));

        if (search.BrandId.HasValue)
            baseSpec.And(new ShopByBrandIdSpec(search.BrandId.Value));
        if (search.ShopManagerId.HasValue)
            baseSpec.And(new ShopByManagerSpec(search.ShopManagerId.Value));
        return baseSpec.GetExpression();
    }

    public ShopSearchSpec(ShopSearchRequest search)
        : base(GetExpression(search))
    {
        AddIncludes(s => s.ShopStatus);
        AddIncludes(s => s.Brand);
        ApplyingPaging(search);
        ApplyOrderByDescending(s => s.CreatedDate);
    }
}
