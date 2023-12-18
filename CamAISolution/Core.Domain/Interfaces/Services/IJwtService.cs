using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.Models.Enums;

namespace Core.Domain.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(Account account, TokenType tokenType);
    bool ValidateToken(string token, TokenType tokenType, string[] roles);
    IList<Claim> GetClaims(string token);

    Guid GetCurrentUserId();
}
