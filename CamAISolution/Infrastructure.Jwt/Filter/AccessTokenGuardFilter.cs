using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter(Role[]? roles, bool allowNew) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;
        var account =
            context.HttpContext.Items[nameof(Account)] as Account ?? throw new UnauthorizedException("Unauthorized");
        var currentRole = account.Roles.Select(ar => ar.Role).ToArray();
        if (roles is { Length: > 0 } && !roles.Intersect(currentRole).Any())
        {
            throw new ForbiddenException("Current user is not allowed");
        }
        if (!allowNew)
        {
            // validate account status is not new
            if (account.AccountStatus == AccountStatus.New)
                throw new NewAccountException(account);
        }
    }
}
