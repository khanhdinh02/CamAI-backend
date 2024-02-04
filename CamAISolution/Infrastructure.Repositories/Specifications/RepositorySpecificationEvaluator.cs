using Core.Domain.Specifications.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Specifications;

/// <summary>
/// Implement of IRepositorySpecificationEvaluator.
/// Purpose of putting this in the Infrastructure layer because AsNoTracking(), Includes() is from QueryableExtensions of EF core.
/// </summary>
/// <typeparam name="T">Type of object to query</typeparam>
public class RepositorySpecificationEvaluator<T> : IRepositorySpecificationEvaluator<T>
    where T : class
{
    public IQueryable<T> GetQuery(IQueryable<T> inputQuery, IRepositorySpecification<T> specification)
    {
        return GetQuery((DbSet<T>)inputQuery, specification);
    }

    private IQueryable<T> GetQuery(DbSet<T> inputQuery, IRepositorySpecification<T> specification)
    {
        var query = inputQuery.AsQueryable();
        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);
        if (specification.IsDisableTracking)
            query = query.AsNoTracking();
        if (specification.Includes != null)
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        if (specification.IncludeStrings != null)
            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
        if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);
        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);
        if (specification.SelectedProperties != null)
            query = query.Select(specification.SelectedProperties);
        if (specification.IsPagingEnabled)
            query = query.Skip(specification.Skip).Take(specification.Take);
        return query;
    }
}
