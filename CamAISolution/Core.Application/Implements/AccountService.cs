using Core.Application.Exceptions;
using Core.Application.Specifications.Accounts;
using Core.Application.Specifications.Accounts.Repositories;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories.Base;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AccountService(IRepository<Account> accountRepo) : IAccountService
{
    public Task<PaginationResult<Account>> GetAccount(Guid? guid = null, DateTime? from = null, DateTime? to = null, int pageSize = 1, int pageIndex = 0)
    {
        var specification = new AccountSearchSpecification(guid, from, to, pageSize, pageIndex);
        return accountRepo.GetAsync(specification);
    }

    public async Task<Account> GetAccountById(Guid id)
    {
        //Normal
        var account = await accountRepo.GetAsync(expression: a => a.Id == id)
            .ContinueWith(t => t.Result.Values.Count > 0 ? t.Result.Values.First() : throw new NotFoundException(typeof(Account), id, this.GetType()));

        //With AccountSearchSpecification
        //account = await accountRepo.GetAsync(new AccountSearchSpecification(guid: id))
        //    .ContinueWith(t => t.Result.Values.Count > 0 ? t.Result.Values.First() : throw new NotFoundException(typeof(Account), id, this.GetType()));


        //With AccountByIdRepoSpecification
        //account = await accountRepo.GetAsync(new AccountByIdRepoSpecification(id))
        //    .ContinueWith(t => t.Result.Values.Count > 0 ? t.Result.Values.First() : throw new NotFoundException(typeof(Account), id, this.GetType()));

        //With AcocuntByIdSpecification
        //account = await accountRepo.GetAsync(expression: new AccountByIdSpecification(id).ToExpression())
        //    .ContinueWith(t => t.Result.Values.Count > 0 ? t.Result.Values.First() : throw new NotFoundException(typeof(Account), id, this.GetType()));

        //Use with another Specification which has same type
        //var combined = new AccountByIdSpecification(id).And(new AccountCreatedFromToSpecification(DateTimeHelper.VNDateTime, DateTimeHelper.VNDateTime.AddDays(10))).ToExpression();
        //account = await accountRepo.GetAsync(expression: combined)
        //    .ContinueWith(t => t.Result.Values.Count > 0 ? t.Result.Values.First() : throw new NotFoundException(typeof(Account), id, this.GetType()));

        return account;
    }
}
