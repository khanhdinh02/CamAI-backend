using System.Linq.Expressions;
using Core.Application.Specifications;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.Entities;

namespace Core.Application;

public class BrandSearchSpec(SearchBrandRequest searchRequest)
    : RepositorySpecification<Brand>(GetExpression(searchRequest))
{
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
