using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public abstract class EntityByIdSpec<T> : RepositorySpec<T> where T : BaseBasicEntity
{
    protected EntityByIdSpec(Expression<Func<T, bool>>? criteria)
        : base(criteria)
    {
        ApplyOrderBy(entity => entity.Id);
        ApplyingPaging(1, 0);
    }
}
