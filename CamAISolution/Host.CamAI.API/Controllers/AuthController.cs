using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO.Auths;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    //TODO [Dat]: Create Register endpoints.

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
}
