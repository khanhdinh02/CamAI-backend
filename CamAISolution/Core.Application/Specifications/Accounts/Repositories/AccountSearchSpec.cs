using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class AccountSearchSpec : RepositorySpecification<Account>
{
    // Use static method to get Expression from ISpecification<Account> object for passing to the RepositorySpecification<Account> when creating object
    private static Expression<Func<Account, bool>> GetExpression(Guid? guid, DateTime? from, DateTime? to)
    {
        var baseSpec = new Specification<Account>();
        if (guid != null)
            baseSpec.And(new AccountByIdSpec(guid.Value));
        if (from != null && to != null)
            baseSpec.And(new AccountCreatedFromToSpec(from.Value, to.Value));
        return baseSpec.GetExpression();
    }

    //TODO: change parameters of constructor to object like: AccountSearchSpecification(AccoutSearch search) : base(GetExpression(search.id, search.From, search.To))
    public AccountSearchSpec(
        Guid? guid = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageSize = 1,
        int pageNumber = 0
    )
        : base(GetExpression(guid, from, to))
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
