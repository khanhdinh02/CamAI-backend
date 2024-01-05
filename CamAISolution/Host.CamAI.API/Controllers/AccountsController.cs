using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IAccountService accountService, IBaseMapping mapper) : ControllerBase
{
    // TODO [Dat]: account search DTO
    [HttpGet]
    public async Task<ActionResult<PaginationResult<Account>>> SampleGetAccounts(
        Guid? guid = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageSize = 1,
        int pageIndex = 0
    )
    {
        // TODO: account DTO
        return Ok(await accountService.GetAccount(guid, from, to, pageSize, pageIndex));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccountById(Guid id)
    {
        // TODO: account DTO
        return Ok(await accountService.GetAccountById(id));
    }

    [HttpPost]
    [AccessTokenGuard]
    public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountDto account)
    {
        var newAccount = await accountService.CreateAccount(account);
        return mapper.Map<Account, AccountDto>(newAccount);
    }
}
