using System.Linq.Expressions;
using Core.Domain.Entities.Base;

namespace Core.Application.Specifications.Repositories;

public abstract class EntityByIdSpec<T, TId> : RepositorySpec<T>
    where T : BaseEntity<TId>
{
    protected EntityByIdSpec(Expression<Func<T, bool>>? criteria)
        : base(criteria)
    {
        ApplyOrderBy(entity => entity.Id);
        ApplyingPaging(1, 0);
    }
}
