using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Services;

namespace Host.CamAI.API.Middlewares;

public class GlobalJwtHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IJwtService jwtService, IAccountService accountService)
    {
        var logger = context.RequestServices.GetRequiredService<IAppLogging<GlobalJwtHandler>>();
        var token = context.Request.Headers.Authorization.ToString();
        var prefix = "Bearer ";
        if (token.StartsWith(prefix))
        {
            token = token.Substring(prefix.Length);
            if (token != string.Empty)
            {
                try
                {
                    var tokenDetails = jwtService.ValidateToken(token, TokenType.AccessToken);
                    var account = await accountService.GetAccountById(tokenDetails.UserId);
                    context.Items[nameof(Account)] = account;
                }
                catch (Exception ex)
                    when (ex is BadRequestException
                        || ex is ForbiddenException
                        || ex is NotFoundException
                        || ex is UnauthorizedException
                    )
                {
                    logger.Info(
                        $"Anonymous do action {context.Connection.RemoteIpAddress} {context.Request.Method} {context.Request.Path}"
                    );
                    // auto = true: indicate client have to refresh the token.
                    context.Response.Headers.Append("auto", $"{true}");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                finally
                {
                    await next(context);
                }
            }
        }
        else
            logger.Info(
                $"Anonymous do action {context.Connection.RemoteIpAddress} {context.Request.Method} {context.Request.Path}"
            );
        await next(context);
    }
}
