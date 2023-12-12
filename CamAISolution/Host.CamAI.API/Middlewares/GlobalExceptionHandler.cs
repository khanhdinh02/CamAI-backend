using System.Net;
using Core.Application.Exceptions.Base;
using Serilog;

namespace Host.CamAI.API.Middlewares;

public class GlobalExceptionHandler(RequestDelegate next, IHostEnvironment env)
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
        Log.Error(ex, ex.Message);
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
