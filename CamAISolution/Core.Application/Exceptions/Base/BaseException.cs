using System.Net;

namespace Core.Application.Exceptions.Base
{
    public class BaseException : Exception
    {
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public BaseException(string errorMessage = "Error occured", HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
    }
}
