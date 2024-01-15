﻿using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class BrandByNameSpec : Specification<Brand>
{
    private readonly string name;

    public BrandByNameSpec(string name)
    {
        this.name = name;
        Expr = GetExpression();
    }

    public override Expression<Func<Brand, bool>> GetExpression() =>
        x => name.Trim().ToLower() == x.Name.Trim().ToLower();
}
