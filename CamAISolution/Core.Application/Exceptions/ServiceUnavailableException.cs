using System.Net;

namespace Core.Application.Exceptions;

public class ServiceUnavailableException(string message) : BaseException(message, HttpStatusCode.ServiceUnavailable) { }
