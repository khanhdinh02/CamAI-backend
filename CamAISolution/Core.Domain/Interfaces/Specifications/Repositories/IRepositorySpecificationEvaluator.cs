namespace Core.Domain.Interfaces.Specifications.Repositories;
public interface IRepositorySpecificationEvaluator<T>
{
    IQueryable<T> GetQuery(IQueryable<T> inputQuery, IRepositorySpecification<T> specification);
}
