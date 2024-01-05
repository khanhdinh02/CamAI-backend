using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace Host.CamAI.API.Middlewares;

public class GlobalJwtHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IJwtService jwtService, IAccountService accountService)
    {
        var token = context.Request.Headers.Authorization.ToString();
        var prefix = "Bearer ";
        if (token.StartsWith(prefix))
        {
            token = token.Substring(prefix.Length);
            if(token != string.Empty)
            {
                try
                {
                    var tokenDetails = jwtService.ValidateToken(token, TokenType.AccessToken);
                    var account = await accountService.GetAccountById(tokenDetails.UserId);
                    context.Items[nameof(Account)] = account;
                }
                catch (Exception ex) when (ex is BadRequestException || ex is ForbiddenException || ex is NotFoundException)
                {
                    context.RequestServices.GetRequiredService<IAppLogging<GlobalJwtHandler>>().Info($"Anonymous do action {context.Connection.RemoteIpAddress} {context.Request.Method} {context.Request.Path}");
                }
            }
        }
        else
            context.RequestServices.GetRequiredService<IAppLogging<GlobalJwtHandler>>().Info($"Anonymous do action {context.Connection.RemoteIpAddress} {context.Request.Method} {context.Request.Path}");
        await next(context);
    }
}
