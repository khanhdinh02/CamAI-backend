using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SampleGetAccounts(Guid? guid = null, DateTime? from = null, DateTime? to = null, int pageSize = 1, int pageIndex = 0)
    {
        return Ok(await accountService.GetAccount(guid, from, to, pageSize, pageIndex));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        return Ok(await accountService.GetAccountById(id));
    }
}
