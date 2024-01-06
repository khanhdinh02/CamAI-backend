using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

/// <summary>
/// Using this to fetching all Classes (Tables) which relating to Account that equal with provied Id.
/// </summary>
/// <param name="id">Query account Id</param>
public class AccountByIdRepoSpec : EntityByIdSpec<Account, Guid>
{
    public AccountByIdRepoSpec(Guid id)
        : base(a => a.Id == id)
    {
        AddIncludes(a => a.Roles);
        //Use string in order to ignore the null warning
        AddIncludes($"{nameof(Account.Brand)}");
        ApplyOrderBy(a => a.CreatedDate);
        //AddIncludes("Shops");
        //AddIncludes("Roles");
    }
}
