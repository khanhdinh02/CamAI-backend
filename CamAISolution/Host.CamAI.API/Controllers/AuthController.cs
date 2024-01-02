using Core.Domain.Models.DTO;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginDto loginDto)
    {
        var tokenResponseDTO = await authService.GetTokensByUsernameAndPassword(loginDto.Username, loginDto.Password);
        return Ok(tokenResponseDTO);
    }

    [HttpPost("refresh")]
    public ActionResult<string> RenewToken(RenewTokenDto renewTokenDto)
    {
        return Ok(authService.RenewToken(renewTokenDto.AccessToken, renewTokenDto.RefreshToken));
    }

    [HttpPost("password")]
    [AccessTokenGuard(true, [ ])]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        await authService.ChangePassword(changePasswordDto);
        return Accepted();
    }
}
