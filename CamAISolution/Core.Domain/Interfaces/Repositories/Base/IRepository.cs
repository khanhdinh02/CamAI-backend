using System.Linq.Expressions;
using Core.Domain.Models;
using Core.Domain.Specifications.Repositories;

namespace Core.Domain.Repositories;

public interface IRepository<T>
{
    public Task<bool> IsExisted(Expression<Func<T, bool>> predicate);
    public Task<bool> IsExisted(object key);
    public Task<T?> GetByIdAsync(params object[] keys);
    public Task<PaginationResult<T>> GetAsync(IRepositorySpecification<T>? specification = null);
    public Task<PaginationResult<T>> GetAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string[]? includeProperties = null,
        bool disableTracking = true,
        bool takeAll = false,
        int pageIndex = 0,
        int pageSize = 5
    );
    public Task<T> AddAsync(T entity);
    public Task<int> CountAsync(Expression<Func<T, bool>>? expression = null);
    public T Update(T entity);
    public T Delete(T entity);
    public IQueryable<IGrouping<TKey, T>> GroupEntity<TKey>(Expression<Func<T, TKey>> expr);
}
