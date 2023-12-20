using System.Net;
using Core.Application.Exceptions.Base;
using Core.Domain;

namespace Host.CamAI.API.Middlewares;

public class GlobalExceptionHandler(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IHostEnvironment env, IAppLogging<GlobalExceptionHandler> logger)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            await ExceptionHandler(context, ex, env);
        }
    }

    private Task ExceptionHandler(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Error occured";
        if (ex is BaseException baseEx)
        {
            message = baseEx.ErrorMessage;
            statusCode = baseEx.StatusCode;
        }
        context.Response.StatusCode = (int)statusCode;
        return context
            .Response
            .WriteAsJsonAsync(
                new
                {
                    Message = message,
                    StatusCode = statusCode,
                    Detailed = env.IsDevelopment() ? ex.Message : ""
                }
            );
    }
}
