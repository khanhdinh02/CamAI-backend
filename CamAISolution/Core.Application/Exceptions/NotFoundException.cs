using Core.Application.Exceptions.Base;

namespace Core.Application.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string errorMessage) : base(errorMessage, System.Net.HttpStatusCode.NotFound) { }
        public NotFoundException(Type target, object key, Type classThrowException) : base($"{classThrowException.Name}: {target.Name}#{key} was not found", System.Net.HttpStatusCode.NotFound) { }
    }
}
