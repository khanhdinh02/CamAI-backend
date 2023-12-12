using System.Security.Claims;
using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Services;

public interface IJwtService
{
    Task<string> GenerateToken(Account account);
    Task ValidateToken(string token);
    IList<Claim> GetClaims(string token);
}
