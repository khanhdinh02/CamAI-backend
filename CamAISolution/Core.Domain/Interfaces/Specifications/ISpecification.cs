using System.Linq.Expressions;

namespace Core.Domain.Interfaces.Specifications;
public interface ISpecification<T>
{
    bool IsSatisfied(T entity);
    Expression<Func<T, bool>> GetExpression();
    ISpecification<T> And(ISpecification<T> specification);
    ISpecification<T> Or(ISpecification<T> specification);
    ISpecification<T> Not(ISpecification<T> specification);
}
