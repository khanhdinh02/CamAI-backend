using System.Linq.Expressions;

namespace Core.Application.Specifications.Repositories;

public abstract class EntityByIdSpec<T> : RepositorySpecification<T>
{
    protected EntityByIdSpec(Expression<Func<T, bool>>? criteria)
        : base(criteria)
    {
        ApplyingPaging(1, 0);
    }
}
