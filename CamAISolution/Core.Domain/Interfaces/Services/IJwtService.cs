using System.Security.Claims;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Services;

public interface IJwtService
{
    string GenerateToken(Guid userId, Role role, AccountStatus? status, TokenType tokenType);
    string GenerateToken(Guid userId, Role role, TokenType tokenType);

    TokenDetailDto ValidateToken(
        string token,
        TokenType tokenType,
        Role[]? acceptableRoles = null,
        bool isValidateTime = true
    );

    IEnumerable<Claim> GetClaims(string token, TokenType tokenType, bool isValidateTime = true);

    Account GetCurrentUser();
    Task SetCurrentUserToSystemHandler();
}
