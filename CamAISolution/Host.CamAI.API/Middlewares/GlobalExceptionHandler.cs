using Core.Application.Exceptions.Base;
using System.Net;

namespace Host.CamAI.API.Middlewares
{
    public class GlobalExceptionHandler(RequestDelegate _next, ILogger<GlobalExceptionHandler> _logger, IHostEnvironment _env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ExpcetionHandler(context, ex);
            }
        }

        private Task ExpcetionHandler(HttpContext context, Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Error occured";
            if (ex is BaseException)
            {
                var baseEx = (BaseException)ex;
                message = baseEx.ErrorMessage;
                statusCode = baseEx.StatusCode;
            }
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsJsonAsync(new
            {
                Message = message,
                StatusCode = statusCode,
                Detailed = _env.IsDevelopment() ? ex.Message : ""
            });
        }
    }
}
