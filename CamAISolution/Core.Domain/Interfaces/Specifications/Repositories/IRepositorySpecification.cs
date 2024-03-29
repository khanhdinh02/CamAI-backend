using System.Linq.Expressions;

namespace Core.Domain.Specifications.Repositories;

/// <summary>
/// Provide method to adjust data when query: including, ordering...
/// </summary>
/// <typeparam name="T">Type of object want to query from database</typeparam>
/// <param name="criteria">Returned object must sastify this criteria</param>
public interface IRepositorySpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object?>>>? Includes { get; }
    List<string>? IncludeStrings { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }

    /// <summary>
    /// Use for select desired properties
    /// </summary>
    /// <remarks>
    /// If you have set this field, please NOTICE that the <see cref="Includes"/> and <see cref="IncludeStrings"/> both will be ignored.
    /// Example:
    /// Includes: [t => t.PropertyOfT]
    /// SelectedProperties: t => new T { AnotherPropertyOfT = t.AnotherPropertyOfT }
    ///
    /// Assert.That(resultOfQuery.PropertyOfT is null)
    /// </remarks>
    Expression<Func<T, T>>? SelectedProperties { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
    bool IsDisableTracking { get; }
    bool IsOrderBySet { get; }
}
