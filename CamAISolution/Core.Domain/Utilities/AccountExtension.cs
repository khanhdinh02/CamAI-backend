using Core.Domain.Entities;

namespace Core.Domain.Utilities;

public static class AccountExtension
{
    public static bool HasRole(this Account account, int roleId) => account.Roles.Any(x => x.Id == roleId);
}
