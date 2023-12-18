using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTOs.Auths;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Jwt.Attribute;

namespace Host.CamAI.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginParams)
    {
        TokenResponseDTO tokenResponseDTO = await authService.GetTokensByUsernameAndPassword(loginParams.Username, loginParams.Password);
        return Ok(tokenResponseDTO);
    }

    [HttpGet]
    [AccessTokenGuard(roles: ["test", "test2", "test3"])]
    public IActionResult TestATGuard()
    {
        return Ok(authService.Test());
    }
}
