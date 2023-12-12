namespace Infrastructure.Jwt;

using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using System.Security.Claims;

public class JwtService(JwtConfiguration configuration) : IJwtService
{
    public Task<string> GenerateToken(Account account)
    {
        throw new NotImplementedException();
    }

    public IList<Claim> GetClaims(string token)
    {
        throw new NotImplementedException();
    }

    public Task ValidateToken(string token)
    {
        throw new NotImplementedException();
    }
}