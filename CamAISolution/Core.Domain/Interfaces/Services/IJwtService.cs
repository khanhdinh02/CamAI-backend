using Core.Domain.Entities;
using System.Security.Claims;

namespace Core.Domain.Interfaces.Services
{
    public interface IJwtService
    {
        Task<string> GenerateToken(Account account);
        Task ValidateToken(string token);
        IList<Claim> GetClaims(string token);
    }
}
