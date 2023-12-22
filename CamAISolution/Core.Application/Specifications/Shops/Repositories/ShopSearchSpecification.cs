using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

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
