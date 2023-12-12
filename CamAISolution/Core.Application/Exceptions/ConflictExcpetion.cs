using Core.Application.Exceptions.Base;

namespace Core.Application.Exceptions
{
    public class ConflictExcpetion(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.Conflict)
    {
    }
}
