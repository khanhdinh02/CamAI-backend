namespace Core.Application.Exceptions;

using Core.Application.Exceptions.Base;

public class BadRequestException(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.BadRequest)
{
}
