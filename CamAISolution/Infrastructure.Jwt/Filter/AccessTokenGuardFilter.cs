using Core.Application.Exceptions;
using Core.Domain.Models.DTO;
using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter(IAccountService accountService, int[] roles, bool allowNew) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var tokenStr = context.HttpContext.Request.Headers.Authorization.ToString();
        var jwtService = context.HttpContext.RequestServices.GetRequiredService(typeof(IJwtService)) as IJwtService;
        if (jwtService == null)
            throw new ServiceUnavailableException("Service is unavailable");

        if (!tokenStr.StartsWith("Bearer "))
            throw new BadRequestException("Missing Bearer or wrong type");

        tokenStr = tokenStr["Bearer ".Length..];
        var token = jwtService.ValidateToken(tokenStr, TokenType.AccessToken, roles);

        if (!allowNew)
        {
            // validate account status is not new
            var account = accountService.GetAccountById(token.UserId).Result;
            if (account.AccountStatusId == AccountStatusEnum.New)
                throw new NewAccountException(account);
        }
    }
}
