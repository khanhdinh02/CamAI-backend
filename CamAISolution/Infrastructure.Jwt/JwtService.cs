using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
using Core.Domain.Services;
using Microsoft.Extensions.Options;

namespace Infrastructure.Jwt;

public class JwtService(IOptions<JwtConfiguration> configuration) : IJwtService
{
    private readonly JwtConfiguration JwtConfiguration = configuration.Value;

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
