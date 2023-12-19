using Core.Domain.Models.DTOs.Auths;

namespace Core.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<TokenResponseDTO> GetTokensByUsernameAndPassword(string username, string password);
    public Guid Test();
}
