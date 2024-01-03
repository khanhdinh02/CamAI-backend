using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.DTO;

namespace Core.Domain.Services;

public interface IJwtService
{
    string GenerateToken(Guid userID, ICollection<Role> roles, TokenTypeEnum tokenType);

    TokenDetailDto ValidateToken(string token, TokenTypeEnum tokenType, int[]? acceptableRoles = null);

    IEnumerable<Claim> GetClaims(string token, TokenTypeEnum tokenType);

    Guid GetCurrentUserId();
}
