using System.Linq.Expressions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Brands.Repositories;

public class BrandSearchSpec : RepositorySpec<Brand>
{
    public BrandSearchSpec(SearchBrandRequest searchRequest)
        : base(GetExpression(searchRequest))
    {
        ApplyingPaging(searchRequest.Size, searchRequest.PageIndex * searchRequest.Size);
    }

    private static Expression<Func<Brand, bool>> GetExpression(SearchBrandRequest searchRequest)
    {
        var baseSpec = new Specification<Brand>();
        if (!string.IsNullOrEmpty(searchRequest.Name))
            baseSpec.And(new BrandByNameSpec(searchRequest.Name));
        if (searchRequest.StatusId.HasValue)
            baseSpec.And(new BrandByStatusSpec(searchRequest.StatusId.Value));
        return baseSpec.GetExpression();
    }
}
