using Core.Application.Exceptions.Base;

namespace Core.Application.Exceptions
{
    public class ConflictExcpetion : BaseException
    {
        public ConflictExcpetion(string errorMessage) : base(errorMessage, System.Net.HttpStatusCode.Conflict) { }
    }
}
