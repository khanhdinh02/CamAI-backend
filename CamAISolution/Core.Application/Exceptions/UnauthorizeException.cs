namespace Core.Application.Exceptions;

public class UnauthorizedException(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.Unauthorized) { }

