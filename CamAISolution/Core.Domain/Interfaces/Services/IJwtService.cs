using System.Security.Claims;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface IJwtService
{
    string GenerateToken(Guid userId, IEnumerable<Role> roles, AccountStatus? status, TokenType tokenType);
    string GenerateToken(Guid userId, IEnumerable<Role> roles, TokenType tokenType);

    TokenDetailDto ValidateToken(
        string token,
        TokenType tokenType,
        int[]? acceptableRoles = null,
        bool isValidateTime = true
    );

    IEnumerable<Claim> GetClaims(string token, TokenType tokenType, bool isValidateTime = true);

    Account GetCurrentUser();
}
