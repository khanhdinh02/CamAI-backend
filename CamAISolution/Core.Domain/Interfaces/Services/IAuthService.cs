using Core.Domain.DTO;

namespace Core.Domain.Services;

public interface IAuthService
{
    Task<TokenResponseDto> GetTokensByUsernameAndPassword(
        string email,
        string password,
        bool isFromMobile,
        string userIp
    );
    public Task<string> RenewToken(string oldAccessToken, string refreshToken, bool isFromMobile, string userIp);

    Task ChangePassword(ChangePasswordDto changePasswordDto);
}
