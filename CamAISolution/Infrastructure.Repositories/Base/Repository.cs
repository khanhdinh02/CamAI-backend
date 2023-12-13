using System.Linq.Expressions;
using Core.Application.Exceptions;
using Core.Domain.Interfaces.Repositories.Base;
using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class Repository<T>(DbContext context) : IRepository<T>
    where T : class
{
    protected readonly DbContext Context = context;

    public virtual async Task<T> AddAsync(T entity)
    {
        var result = await Context.Set<T>().AddAsync(entity);
        return result.Entity;
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
    {
        return expression == null ? await Context.Set<T>().CountAsync() : await Context.Set<T>().CountAsync(expression);
    }

    public virtual T Delete(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
        }
        return entity;
    }

    public virtual async Task<PaginationResult<T>> GetAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string[]? includeProperties = null,
        bool disableTracking = true,
        bool takeAll = false,
        int pageIndex = 0,
        int pageSize = 5
    )
    {
        IQueryable<T> query = Context.Set<T>();
        var paginationResult = new PaginationResult<T> { TotalCount = await CountAsync(expression) };
        if (expression != null)
            query = query.Where(expression);
        if (disableTracking)
            query = query.AsNoTracking();
        if (includeProperties is { Length: > 0 })
        {
            foreach (var includeItem in includeProperties)
                query = query.Include(includeItem);
            query = query.AsSplitQuery();
        }
        if (takeAll)
        {
            if (orderBy != null)
                paginationResult.Values = await orderBy(query).ToListAsync();
            else
                paginationResult.Values = await query.ToListAsync();
        }
        else
        {
            paginationResult.PageIndex = pageIndex;
            if (orderBy == null)
                paginationResult.Values = await query.Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();
            else
                paginationResult.Values = await orderBy(query).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }
        paginationResult.PageIndex = pageIndex;
        paginationResult.PageSize = pageSize;
        return paginationResult;
    }

    public virtual async Task<T> GetByIdAsync(object key)
    {
        return await Context.Set<T>().FindAsync(key) ?? throw new NotFoundException(typeof(T), key, GetType());
    }

    public virtual T Update(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }
        return entity;
    }
}
