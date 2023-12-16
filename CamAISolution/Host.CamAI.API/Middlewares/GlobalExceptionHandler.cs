using System.Net;
using Core.Application.Exceptions.Base;
using Core.Domain;

namespace Host.CamAI.API.Middlewares;

public class GlobalExceptionHandler(RequestDelegate next, IHostEnvironment env, IAppLogging<GlobalExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await ExceptionHandler(context, ex);
        }
    }

    private Task ExceptionHandler(HttpContext context, Exception ex)
    {
        logger.Error(ex.Message, ex);
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
