using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IAccountService accountService, IBaseMapping mapper) : ControllerBase
{
    /// <summary>
    /// Search accounts
    /// </summary>
    /// <remarks>
    /// <para>
    /// Admin can get all accounts<br/>
    /// Brand manager can get all accounts working for their brand (the BrandId field is ignored)<br/>
    /// </para>
    /// <para><c>Search</c> can be Email, Name or Phone</para>
    /// </remarks>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<ActionResult<PaginationResult<AccountDto>>> GetAccounts([FromQuery] SearchAccountRequest req)
    {
        var accounts = await accountService.GetAccounts(req);
        return mapper.Map<Account, AccountDto>(accounts);
    }

    /// <summary>
    /// Get an account by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccountById(Guid id)
    {
        var account = await accountService.GetAccountById(id);
        return mapper.Map<Account, AccountDto>(account);
    }

    /// <summary>
    /// Create new account
    /// </summary>
    /// <remarks>
    /// Admin can create Brand Manager and Technician<br/>
    /// Brand Manager can create Shop Manager<br/>
    /// Creation for Employee is not yet supported
    /// </remarks>
    /// <param name="account"></param>
    /// <returns>The created account</returns>
    [HttpPost]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountDto account)
    {
        var newAccount = await accountService.CreateAccount(account);
        return mapper.Map<Account, AccountDto>(newAccount);
    }

    /// <summary>
    /// Update FCM token for receiving messaging (notification)
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPatch("notifications/firebase/{token}")]
    [AccessTokenGuard]
    public async Task<IActionResult> UpdateFcmToken(string token)
    {
        await accountService.UpdateAccountFcmToken(token);
        return Ok();
    }

    /// <summary>
    /// Update account
    /// </summary>
    /// <remarks>
    /// Admin can update Brand Manager and Technician<br/>
    /// Brand Manager can update Shop Manager
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="account"></param>
    /// <returns>The updated account</returns>
    [HttpPut("{id}")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<ActionResult<AccountDto>> UpdateAccount(Guid id, UpdateAccountDto account)
    {
        var updatedAccount = await accountService.UpdateAccount(id, account);
        return mapper.Map<Account, AccountDto>(updatedAccount);
    }

    /// <summary>
    /// Delete account
    /// </summary>
    /// <remarks>
    /// Admin can delete Brand Manager and Technician<br/>
    /// Brand Manager can delete Shop Manager
    /// </remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<ActionResult> DeleteAccount(Guid id)
    {
        await accountService.DeleteAccount(id);
        return Accepted();
    }
}
