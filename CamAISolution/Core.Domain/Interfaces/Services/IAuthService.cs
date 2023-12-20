using Core.Domain.Models.DTOs.Auths;

namespace Core.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<TokenResponseDto> GetTokensByUsernameAndPassword(string username, string password);
    public Guid Test();
    public string RenewToken(string oldAccessToken, string refreshToken);
}
