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
    [HttpGet]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager, RoleEnum.ShopManager)]
    public async Task<ActionResult<PaginationResult<AccountDto>>> GetAccounts([FromQuery] SearchAccountRequest req)
    {
        var accounts = await accountService.GetAccounts(req);
        return mapper.Map<Account, AccountDto>(accounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccountById(Guid id)
    {
        var account = await accountService.GetAccountById(id);
        return mapper.Map<Account, AccountDto>(account);
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.BrandManager)]
    public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountDto account)
    {
        var newAccount = await accountService.CreateAccount(account);
        return mapper.Map<Account, AccountDto>(newAccount);
    }
}
