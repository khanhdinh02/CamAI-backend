namespace Core.Application.Exceptions.Base;
using System.Net;

public class BaseException(string errorMessage = "Error occured", HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : Exception
{
    public string ErrorMessage { get; } = errorMessage;
    public HttpStatusCode StatusCode { get; } = statusCode;
}
