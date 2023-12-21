﻿using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Models.DTOs.Shops;

namespace Core.Application.Specifications.Repositories;

public class SearchShopSpecification : RepositorySpecification<Shop>
{
    private static Expression<Func<Shop, bool>> GetExpression(SearchShopRequest search)
    {
        var baseSpec = new Specification<Shop>();
        if (!string.IsNullOrEmpty(search.Name))
            baseSpec.And(new ShopByNameSpecification(search.Name));
        if (search.StatusId.HasValue)
            baseSpec.And(new ShopByStatusSpecification(search.StatusId.Value));
        return baseSpec.GetExpression();
    }

    public SearchShopSpecification(SearchShopRequest search)
        : base(GetExpression(search))
    {
        AddIncludes(s => s.ShopStatus);
        ApplyingPaging(search.Size, search.PageIndex * search.Size);
        ApplyOrderByDescending(s => s.CreatedDate);
    }
}
