using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Core.Application.Exceptions;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter : IAuthorizationFilter
{

    readonly string[] roles;

    public AccessTokenGuardFilter(string[] roles)
    {
        this.roles = roles;
    }


    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context != null)
        {
            

            StringValues token = context.HttpContext.Request.Headers.Authorization;
            IJwtService? jwtService = context.HttpContext.RequestServices.GetRequiredService(typeof(IJwtService)) as IJwtService;
            try
            {
                bool isTokenValid = jwtService.ValidateToken(token, TokenType.ACCESS_TOKEN, roles);
            }
            catch (UnauthorizeException ex)
            {

            }
            /*if (!isTokenValid)
            {
                //TODO: CREATE ERROR HANDLER FOR TOKEN INVALID
                throw new UnauthorizeException("test");
            */}
            // Auth logic
        }
    }
}