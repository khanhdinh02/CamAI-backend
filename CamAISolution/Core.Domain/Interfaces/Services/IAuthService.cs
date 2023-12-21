using Core.Domain.Models.DTO.Auths;

namespace Core.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<TokenResponseDto> GetTokensByUsernameAndPassword(string username, string password);
    public string RenewToken(string oldAccessToken, string refreshToken);
}
