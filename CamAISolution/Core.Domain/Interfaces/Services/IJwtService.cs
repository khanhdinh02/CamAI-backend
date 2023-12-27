using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.Models.DTO.Accounts;
using Core.Domain.Models.DTO.Auths;

namespace Core.Domain.Services;

public interface IJwtService
{
    string GenerateToken(Guid userID, ICollection<Role> roles, TokenTypeEnum tokenType);

    TokenDetailDto ValidateToken(string token, TokenTypeEnum tokenType, Guid[]? acceptableRoles = null);

    IEnumerable<Claim> GetClaims(string token, TokenTypeEnum tokenType);

    Guid GetCurrentUserId();
}
