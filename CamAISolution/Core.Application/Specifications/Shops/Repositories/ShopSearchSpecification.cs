using System.Linq.Expressions;
using Core.Domain;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class SearchShopSpecification : RepositorySpecification<Shop>
{
    private static Expression<Func<Shop, bool>> GetExpression(SearchShopRequest search)
    {
        var baseSpec = new Specification<Shop>();
        if (!String.IsNullOrEmpty(search.Name))
            baseSpec.And(new ShopByNameSpecification(search.Name));
        if (!String.IsNullOrEmpty(search.Status))
            baseSpec.And(new ShopByStatusSpecification(search.Status));
        return baseSpec.GetExpression();
    }

    public SearchShopSpecification(SearchShopRequest search) : base(GetExpression(search))
    {
        ApplyingPaging(search.Size, search.PageIndex * search.Size);
        ApplyOrderByDescending(s => s.CreatedDate);
    }
}
