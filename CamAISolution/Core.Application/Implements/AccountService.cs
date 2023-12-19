using System.Security.Cryptography;
using Core.Application.Exceptions;
using Core.Application.Specifications.Accounts;
using Core.Application.Specifications.Accounts.Repositories;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories.Base;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.dtos.auth;
using Core.Domain.Utilities;
using Core.Domain.Models.enums;

namespace Core.Application.Implements;

public class AccountService() : IAccountService
{
    public Task<PaginationResult<Account>> GetAccount(Guid? guid = null, DateTime? from = null, DateTime? to = null, int pageSize = 1, int pageIndex = 0)
    {
        /* var specification = new AccountSearchSpecification(guid, from, to, pageSize, pageIndex);
         return accountRepo.GetAsync(specification);*/
        throw new NotImplementedException();
    }

    public async Task<Account> GetAccountById(Guid id)
    {
        //Normal
        /*var accounts = await accountRepo.GetAsync(expression: a => a.Id == id);

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

        return accounts.Values.First();*/
        throw new NotImplementedException();
    }

    



}

