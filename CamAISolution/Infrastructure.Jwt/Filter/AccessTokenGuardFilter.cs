using Core.Application.Exceptions;
using Core.Domain.Models.Enums;
using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter(Guid[] roles) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers.Authorization.ToString();
        var jwtService =
            context.HttpContext.RequestServices.GetRequiredService(typeof(IJwtService)) as IJwtService
            ?? throw new NullReferenceException($"Null object of {nameof(IJwtService)} type");

        if (!token.StartsWith("Bearer "))
            throw new BadRequestException("Missing Bearer or wrong type");

        token = token.Substring("Bearer ".Length);
        jwtService.ValidateToken(token, TokenType.AccessToken, roles);
    }
}
