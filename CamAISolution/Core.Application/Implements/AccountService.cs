using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application;

public class AccountService(IRepository<Account> accountRepo) : IAccountService
{
    public Task<PaginationResult<Account>> GetAccount(
        Guid? guid = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageSize = 1,
        int pageIndex = 0
    )
    {
        var specification = new AccountSearchSpec(guid, from, to, pageSize, pageIndex);
        return accountRepo.GetAsync(specification);
    }

    public async Task<Account> GetAccountById(Guid id)
    {
        //Normal
        var accounts = await accountRepo.GetAsync(expression: a => a.Id == id);

        //With AccountSearchSpecification
        //accounts = await accountRepo.GetAsync(new AccountSearchSpecification(guid: id));


        //With AccountByIdRepoSpecification
        //accounts = await accountRepo.GetAsync(new AccountByIdRepoSpecification(id));

        //With AcocuntByIdSpecification
        //accounts = await accountRepo.GetAsync(expression: new AccountByIdSpecification(id).ToExpression());

        //Use with another Specification which has same type
        //var combined = new AccountByIdSpecification(id).And(new AccountCreatedFromToSpecification(DateTimeHelper.VNDateTime, DateTimeHelper.VNDateTime.AddDays(10))).ToExpression();
        //accounts = await accountRepo.GetAsync(expression: combined);

        if (accounts.Values.Count <= 0)
            throw new NotFoundException(typeof(Account), id, this.GetType());

        return accounts.Values[0];
    }
}
