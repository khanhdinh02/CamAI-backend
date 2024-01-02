using Core.Domain.Entities;
using Core.Domain.Entities.Base;

namespace Core.Application.Exceptions;

public class ForbiddenException(string errorMessage) : BaseException(errorMessage, System.Net.HttpStatusCode.Forbidden)
{
    public ForbiddenException(Account account, BaseEntity<int> baseEntity)
        : this(
            $"Account id {account.Id} trying to access restricted resources of ${baseEntity.GetType().Name} id ${baseEntity.Id}"
        ) { }

    public ForbiddenException(Account account, BaseEntity<Guid> baseEntity)
        : this(
            $"Account id {account.Id} trying to access restricted resources of ${baseEntity.GetType().Name} id ${baseEntity.Id}"
        ) { }

    public ForbiddenException(Account account, Type targetType)
        : this($"Account id {account.Id} has insufficient permission for resource ${targetType.Name}") { }
}
