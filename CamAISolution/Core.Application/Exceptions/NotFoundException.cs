namespace Core.Application.Exceptions;

public class NotFoundException(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.NotFound)
{
    public NotFoundException(Type target, object key, Type? classThrowException = null)
        : this($"{classThrowException?.Name.Concat(": ")}{target.Name}#{key} was not found") { }
}
