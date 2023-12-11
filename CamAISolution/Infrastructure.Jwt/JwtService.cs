using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using System.Security.Claims;

namespace Infrastructure.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly AppConfiguration configuration;

        public JwtService(AppConfiguration configuration)
        {
            this.configuration = configuration;
        }

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
}