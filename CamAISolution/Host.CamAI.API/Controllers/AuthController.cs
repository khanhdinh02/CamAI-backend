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
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var tokenResponseDTO = await authService.GetTokensByUsernameAndPassword(loginDto.Username, loginDto.Password);
        return Ok(tokenResponseDTO);
    }

    [HttpGet]
    [AccessTokenGuard(roles: ["test4", "test2", "test3"])]
    public IActionResult TestATGuard()
    {
        return Ok(authService.Test());
    }

    [HttpPost("refresh")]
    public IActionResult RenewToken(RenewTokenDto renewTokenDto)
    {
        return Ok(authService.RenewToken(renewTokenDto.AccessToken, renewTokenDto.RefreshToken));
    }
}
