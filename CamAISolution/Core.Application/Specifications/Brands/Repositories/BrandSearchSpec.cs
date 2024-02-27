using System.Linq.Expressions;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class BrandSearchSpec : RepositorySpec<Brand>
{
    public BrandSearchSpec(SearchBrandRequest searchRequest)
        : base(GetExpression(searchRequest))
    {
        ApplyingPaging(searchRequest);
        AddIncludes(b => b.BrandManager!);
        AddIncludes(nameof(Brand.Logo));
        AddIncludes(nameof(Brand.Banner));
    }

    private static Expression<Func<Brand, bool>> GetExpression(SearchBrandRequest searchRequest)
    {
        var baseSpec = new Specification<Brand>();
        if (!string.IsNullOrEmpty(searchRequest.Name))
            baseSpec.And(new BrandByNameSpec(searchRequest.Name));
        if (searchRequest.BrandStatus.HasValue)
            baseSpec.And(new BrandByStatusSpec(searchRequest.BrandStatus.Value));
        if (searchRequest.BrandId.HasValue)
            baseSpec.And(new BrandByIdSpec(searchRequest.BrandId.Value));
        if (searchRequest.HasManager.HasValue)
            baseSpec.And(new Specification<Brand>(b => searchRequest.HasManager.Value == (b.BrandManagerId != null)));
        return baseSpec.GetExpression();
    }
}
