using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Core.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Core.Domain.Models.enums;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

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
            
            StringValues token =  context.HttpContext.Request.Headers.Authorization;
            IJwtService? jwtService = context.HttpContext.RequestServices.GetRequiredService(typeof(IJwtService)) as IJwtService;
            bool isTokenValid = jwtService.ValidateToken(token, TokenType.ACCESS_TOKEN, roles);
            if (!isTokenValid)
            {
                //TODO: CREATE ERROR HANDLER FOR TOKEN INVALID
                throw new Exception("Token invalid");
            }
            // Auth logic
        }
    }
}