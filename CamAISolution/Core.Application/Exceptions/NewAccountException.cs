using Core.Domain.Entities;

namespace Core.Application.Exceptions;

public class NewAccountException(Account account)
    : BaseException(
        $"Account {account.Id} is new, please change password before accessing resources",
        System.Net.HttpStatusCode.PreconditionFailed
    );
