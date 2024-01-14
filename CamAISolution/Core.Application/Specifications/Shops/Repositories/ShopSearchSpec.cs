using System.Linq.Expressions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Shops.Repositories;

public class ShopSearchSpec : RepositorySpec<Shop>
{
    private static Expression<Func<Shop, bool>> GetExpression(ShopSearchRequest search)
    {
        var baseSpec = new Specification<Shop>();
        if (!string.IsNullOrEmpty(search.Name))
            baseSpec.And(new ShopByNameSpec(search.Name));
        if (!string.IsNullOrEmpty(search.Phone))
            baseSpec.And(new ShopByPhoneSpec(search.Phone));

        if (search.StatusId.HasValue)
            baseSpec.And(new ShopByStatusSpec(search.StatusId.Value));

        if (search.BrandId.HasValue)
            baseSpec.And(new ShopByBrandIdSpec(search.BrandId.Value));

        if (search.WardId.HasValue)
            baseSpec.And(new ShopByWardSpec(search.WardId.Value));

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
