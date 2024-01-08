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
        AddIncludes(a => a.AccountStatus);
        AddIncludes(nameof(Account.Brand));
        AddIncludes(a => a.Ward!.District.Province);
        AddIncludes(a => a.ManagingShop!);
        AddIncludes(a => a.WorkingShop!);
    }
}
