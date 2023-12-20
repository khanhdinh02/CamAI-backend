using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTOs.Auths;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginParams)
    {
        var tokenResponseDTO = await authService.GetTokensByUsernameAndPassword(
            loginParams.Username,
            loginParams.Password
        );
        return Ok(tokenResponseDTO);
    }

    [HttpGet]
    [AccessTokenGuard(roles: ["test4", "test2", "test3"])]
    public IActionResult TestATGuard()
    {
        return Ok(authService.Test());
    }

    [HttpPost("renew")]
    public IActionResult RenewToken(RenewTokenParam param)
    {
        return Ok(authService.RenewToken(param.AccessToken, param.RefreshToken));
    }
}
