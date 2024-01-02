using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.Models.DTO;

namespace Core.Domain.Services;

public interface IJwtService
{
    string GenerateToken(Guid userId, IEnumerable<Role> roles, AccountStatus? status, TokenType tokenType);
    string GenerateToken(Guid userId, IEnumerable<Role> roles, TokenType tokenType);

    TokenDetailDto ValidateToken(string token, TokenType tokenType, int[]? acceptableRoles = null);

    IEnumerable<Claim> GetClaims(string token, TokenType tokenType);

    Guid GetCurrentUserId();
}
