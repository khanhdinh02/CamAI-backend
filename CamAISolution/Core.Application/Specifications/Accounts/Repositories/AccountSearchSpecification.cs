using System.Linq.Expressions;
using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Accounts.Repositories;

public class AccountSearchSpecification : RepositorySpecification<Account>
{
    // Use static method to get Expresion from ISpecification<Acccount> object for passing to the RepositorySpecification<Account> when creating object
    private static Expression<Func<Account, bool>> GetExpression(Guid? guid, DateTime? from, DateTime? to)
    {
        var baseSpec = new Specification<Account>();
        if (guid != null)
            baseSpec.And(new AccountByIdSpecification(guid.Value));
        if (from != null && to != null)
            baseSpec.And(new AccountCreatedFromToSpecification(from.Value, to.Value));
        return baseSpec.GetExpression();

    }

    //TODO: change parameters of constructor to object like: AccountSearchSpecification(AccoutSearch search) : base(GetExpression(search.id, search.From, search.To))
    public AccountSearchSpecification(Guid? guid = null, DateTime? from = null, DateTime? to = null, int pageSize = 1, int pageNumber = 0) : base(GetExpression(guid, from, to))
    {
        base.ApplyingPaging(pageSize, (pageNumber - 1) * pageSize);
        base.DisableTracking();
        base.ApplyOrderByDescending(a => a.CreatedDate);
        //base.ApplyOrderBy(a => a.CreatedDate);
        //base.AddIncludes("Shops");
        //base.AddIncludes("Roles");
        //base.AddIncludes(a => a.Roles);
    }
}