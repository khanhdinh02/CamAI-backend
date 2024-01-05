using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter(int[]? roles, bool allowNew) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<IAppLogging<AccessTokenGuardFilter>>();
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;
        var account = context.HttpContext.Items[nameof(Account)] as Account ?? throw new UnauthorizedException("Unauthorized");
        var currentRole = account.Roles.Select(r => r.Id).ToArray();
        if(roles != null && roles.Count() > 0 && !roles.Intersect(currentRole).Any())
        {
            throw new ForbiddenException("Current user is not allowed");
        }
        if (!allowNew)
        {
            // validate account status is not new
            if (account.AccountStatusId == AccountStatusEnum.New)
                throw new NewAccountException(account);
        }
    }
}
