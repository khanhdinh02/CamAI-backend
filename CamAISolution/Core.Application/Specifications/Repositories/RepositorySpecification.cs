using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Specifications.Repositories;

namespace Core.Application.Specifications.Repositories;

/// <summary>
/// Implement of IRepositorySpecification
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="criteria"></param>
public abstract class RepositorySpec<T>(Expression<Func<T, bool>>? criteria = null) : IRepositorySpecification<T>
{
    public Expression<Func<T, bool>>? Criteria => criteria;
    public List<Expression<Func<T, object>>>? Includes { get; } = new List<Expression<Func<T, object>>>();
    public List<string>? IncludeStrings { get; } = new List<string>();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; } = false;
    public bool IsDisableTracking { get; private set; } = false;
    public bool IsOrderBySet { get; private set; } = false;

    protected virtual void AddIncludes(Expression<Func<T, object>> include)
    {
        Includes?.Add(include);
    }

    protected virtual void AddIncludes(params Expression<Func<T, object>>[] include)
    {
        Includes?.AddRange(include);
    }

    protected virtual void AddIncludes(string include)
    {
        IncludeStrings?.Add(include);
    }

    protected virtual void ApplyingPaging(int take, int skip)
    {
        Take = take;
        Skip = skip;
        IsPagingEnabled = true;
    }

    protected virtual void ApplyingPaging(BaseSearchRequest searchRequest)
    {
        ApplyingPaging(searchRequest.Size, searchRequest.PageIndex * searchRequest.Size);
    }

    protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
        IsOrderBySet = true;
    }

    protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
        IsOrderBySet = true;
    }

    protected virtual void DisableTracking()
    {
        IsDisableTracking = true;
    }
}
