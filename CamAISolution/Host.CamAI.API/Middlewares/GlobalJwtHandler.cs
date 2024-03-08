using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using Host.CamAI.API.Models;

namespace Host.CamAI.API.Middlewares;

public class GlobalJwtHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IJwtService jwtService, IAccountService accountService)
    {
        var logger = context.RequestServices.GetRequiredService<IAppLogging<GlobalJwtHandler>>();
        var token = GetTokenFromRequest(context);
        if (token != string.Empty)
        {
            try
            {
                var tokenDetails = jwtService.ValidateToken(token, TokenType.AccessToken);
                var account = await accountService.GetAccountById(tokenDetails.UserId);
                context.Items[nameof(Account)] = account;
            }
            catch (Exception ex)
                when (ex is BadRequestException or ForbiddenException or NotFoundException or UnauthorizedException)
            {
                logger.Info(
                    $"Anonymous do action {context.Connection.RemoteIpAddress} {context.Request.Method} {context.Request.Path}"
                );
                if (context.Request.Path.HasValue && !context.Request.Path.Value.ToLower().Contains("auth/refresh"))
                    // auto = true: indicate client have to refresh the token.
                    context.Response.Headers.Append(HeaderNameConstant.Auto, $"{true}");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        else
            logger.Info(
                $"Anonymous do action {context.Connection.RemoteIpAddress} {context.Request.Method} {context.Request.Path}"
            );
        await next(context);
    }

    private static string GetTokenFromRequest(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            context.Response.Headers.SecWebSocketProtocol = "Bearer";
            var author = context.Request.Headers.SecWebSocketProtocol.ToString().Split(",");
            if (author.Length < 2 || author[0].Trim() != "Bearer")
                return "";
            return author[1].Trim();
        }

        var token = context.Request.Headers.Authorization.ToString();
        const string prefix = "Bearer ";
        return token.StartsWith(prefix) ? token[prefix.Length..] : "";
    }
}
