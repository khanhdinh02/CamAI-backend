using Core.Application.Exceptions.Base;

namespace Core.Application.Exceptions;

public class BadRequestException(string errorMessage)
    : BaseException(errorMessage, System.Net.HttpStatusCode.BadRequest) { }
