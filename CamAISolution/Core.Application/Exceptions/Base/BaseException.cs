using System.Net;

namespace Core.Application.Exceptions.Base;

public class BaseException(
    string errorMessage = "Error occured",
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError
) : Exception
{
    public string ErrorMessage { get; } = errorMessage;
    public HttpStatusCode StatusCode { get; } = statusCode;
}
