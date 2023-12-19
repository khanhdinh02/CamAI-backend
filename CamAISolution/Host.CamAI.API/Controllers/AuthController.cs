using Core.Domain.Interfaces.Services;
using Core.Domain.Models.dtos.auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Jwt.Attribute;

namespace Host.CamAI.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> login(LoginDTO loginParams)
    {
        TokenResponseDTO tokenResponseDTO = await authService.getTokensByUsernameAndPassword(loginParams.Username, loginParams.Password);
        return Ok(tokenResponseDTO);
    }

    [HttpGet]
    [AccessTokenGuard(roles: ["test", "test2", "test3"])]
    public async Task<IActionResult> TestATGuard()
    {
        
        return Ok(authService.test());
    }
}
