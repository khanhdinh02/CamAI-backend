namespace Core.Application.Exceptions;
public class ForbiddenException(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.Forbidden) { }
