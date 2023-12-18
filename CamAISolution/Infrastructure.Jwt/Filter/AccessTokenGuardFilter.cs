using Microsoft.AspNetCore.Mvc.Filters;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Core.Application.Exceptions;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter : IAuthorizationFilter
{

    readonly string[] roles;

    public AccessTokenGuardFilter(string[] roles)
    {
        this.roles = roles;
    }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string token = context.HttpContext.Request.Headers.Authorization.ToString();
        IJwtService jwtService = context.HttpContext.RequestServices.GetRequiredService(typeof(IJwtService)) as IJwtService ?? throw new NullReferenceException($"Null object of {nameof(IJwtService)} type");
        bool isTokenValid = jwtService.ValidateToken(token, TokenType.ACCESS_TOKEN, roles);
        if (!isTokenValid)
        {
            //TODO: CREATE ERROR HANDLER FOR TOKEN INVALID
            throw new UnauthorizeException("Token invalid");
        }
        // Auth logic
    }
}