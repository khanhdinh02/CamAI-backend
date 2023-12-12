namespace Core.Application.Exceptions;

using Core.Application.Exceptions.Base;

public class NotFoundException(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.NotFound)
{
    public NotFoundException(Type target, object key, Type classThrowException) : this($"{classThrowException.Name}: {target.Name}#{key} was not found") { }
}
