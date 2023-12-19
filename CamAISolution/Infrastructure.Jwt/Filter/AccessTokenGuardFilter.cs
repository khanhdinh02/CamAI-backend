using Core.Application.Exceptions;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter(string[] roles) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers.Authorization.ToString();
        var jwtService =
            context.HttpContext.RequestServices.GetRequiredService(typeof(IJwtService)) as IJwtService
            ?? throw new NullReferenceException($"Null object of {nameof(IJwtService)} type");
        var isTokenValid = jwtService.ValidateToken(token, TokenType.AccessToken, roles);
        if (!isTokenValid)
        {
            //TODO: CREATE ERROR HANDLER FOR TOKEN INVALID
            throw new UnauthorizedException("Token invalid");
        }
        // Auth logic
    }
}
