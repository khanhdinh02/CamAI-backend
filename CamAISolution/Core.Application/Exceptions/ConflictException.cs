namespace Core.Application.Exceptions;

public class ConflictException(string errorMessage)
    : BaseException(errorMessage, System.Net.HttpStatusCode.Conflict) { }
