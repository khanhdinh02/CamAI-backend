using Core.Application.Exceptions.Base;

namespace Core.Application;

public class ServiceUnavailableException(string message) : BaseException(message, System.Net.HttpStatusCode.ServiceUnavailable)
{

}
