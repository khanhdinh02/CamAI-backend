using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController(IAccountService accountService, IBaseMapping mapper) : ControllerBase
{
    [AccessTokenGuard]
    [HttpGet]
    public async Task<AccountDto> GetProfile()
    {
        return await Task.FromResult(mapper.Map<Account, AccountDto>(accountService.GetCurrentAccount()));
    }

    [AccessTokenGuard]
    [HttpPut]
    public async Task<AccountDto> UpdateProfile(UpdateProfileDto dto)
    {
        return mapper.Map<Account, AccountDto>(await accountService.UpdateProfile(dto));
    }
}
