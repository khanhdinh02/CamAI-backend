using Core.Domain.DTO;

namespace Core.Domain.Services;

public interface IAuthService
{
    Task<TokenResponseDto> GetTokensByUsernameAndPassword(string email, string password);
    public string RenewToken(string oldAccessToken, string refreshToken);

    Task ChangePassword(ChangePasswordDto changePasswordDto);
}
