using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Utilities;

public static class AccountExtension
{
    public static bool HasRole(this Account account, Role role) => account.Roles.Any(ar => ar.Role == role);
}
