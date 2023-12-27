using System.Net;

namespace Core.Application.Exceptions;

public class BaseException(
    string errorMessage = "Error occured",
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError
) : Exception
{
    public string ErrorMessage { get; } = errorMessage;
    public HttpStatusCode StatusCode { get; } = statusCode;
}
