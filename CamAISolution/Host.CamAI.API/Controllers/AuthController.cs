using Core.Domain.DTO;
using Core.Domain.Services;
using Host.CamAI.API.Utils;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    /// <summary>
    /// Set User-Agent header to Mobile if login with <c>Mobile</c> (Case sensitive)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginDto loginDto)
    {
        logger.LogInformation($"Request's IP: {HttpContext.Connection.RemoteIpAddress} for login");
        var tokenResponseDto = await authService.GetTokensByUsernameAndPassword(
            loginDto.Username,
            loginDto.Password,
            HttpUtilities.IsFromMobile(Request),
            HttpUtilities.UserIp(HttpContext)
        );
        return Ok(tokenResponseDto);
    }

    /// <summary>
    /// Set User-Agent header to Mobile if refresh with <c>Mobile</c> (Case sensitive)
    /// </summary>
    [HttpPost("refresh")]
    public async Task<ActionResult<string>> RenewToken(RenewTokenDto renewTokenDto)
    {
        logger.LogInformation($"Request's IP: {HttpContext.Connection.RemoteIpAddress} for refresh token");

        return Ok(
            await authService.RenewToken(
                renewTokenDto.AccessToken,
                renewTokenDto.RefreshToken,
                HttpUtilities.IsFromMobile(Request),
                HttpUtilities.UserIp(HttpContext)
            )
        );
    }

    [HttpPost("password")]
    [AccessTokenGuard(true, [])]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        await authService.ChangePassword(changePasswordDto);
        return Accepted();
    }
}
