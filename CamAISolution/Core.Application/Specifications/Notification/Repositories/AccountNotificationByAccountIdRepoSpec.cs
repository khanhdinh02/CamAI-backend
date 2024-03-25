using Core.Domain.Entities;

namespace Core.Application.Specifications.Repositories;

public class AccountNotificationByAccountIdRepoSpec : RepositorySpec<AccountNotification>
{
    public AccountNotificationByAccountIdRepoSpec(Guid id)
        : base(an => an.AccountId == id)
    {
        AddIncludes(a => a.Account);
        AddIncludes(a => a.Notification);
    }
}
