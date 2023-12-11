using Core.Application.Exceptions.Base;

namespace Core.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, System.Net.HttpStatusCode.BadRequest) { }
    }
}
