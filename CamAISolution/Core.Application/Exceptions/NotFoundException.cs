namespace Core.Application.Exceptions;

public class NotFoundException(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.NotFound)
{
    public NotFoundException(Type target, object key)
        : this($"{target.Name}#{key} was not found") { }
}
