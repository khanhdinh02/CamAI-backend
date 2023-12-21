using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

/// <summary>
/// Using this to fetching all Classes (Tables) which relating to Account that equal with provied Id.
/// </summary>
/// <param name="id">Query account Id</param>
public class AccountByIdRepoSpec : RepositorySpecification<Account>
{
    public AccountByIdRepoSpec(Guid id)
        : base(a => a.Id == id)
    {
        ApplyingPaging(1, 0);
        ApplyOrderBy(a => a.CreatedDate);
        //AddIncludes("Shops");
        //AddIncludes("Roles");
    }
}
