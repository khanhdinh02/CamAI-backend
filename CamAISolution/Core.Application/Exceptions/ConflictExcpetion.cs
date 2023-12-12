namespace Core.Application.Exceptions;

using Core.Application.Exceptions.Base;

public class ConflictExcpetion(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.Conflict)
{
}
