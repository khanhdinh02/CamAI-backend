using Core.Domain.Entities.Base;

namespace Infrastructure.Repositories.Utils;
public static class QueryHelper
{
    public static IOrderedQueryable<T> SetDefaultOrderBy<T>(IQueryable<T> query)
    {
        if (typeof(BusinessEntity).IsAssignableFrom(typeof(T)))
            return query.OrderByDescending(e => (e as BusinessEntity)!.CreatedDate);
        // Order by the primary key of entity
        return query.OrderBy(e => e);
    }
}
