using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Core.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Jwt;

public class JwtService(IOptions<JwtConfiguration> configuration, IServiceProvider serviceProvider, IAppLogging<JwtService> logger) : IJwtService
{
    private readonly JwtConfiguration JwtConfiguration = configuration.Value;

    public string GenerateToken(Account account, TokenType tokenType)
    {
        int tokenDurationInMinute;
        bool isAccessTokenType = tokenType == TokenType.ACCESS_TOKEN;
        string jwtSecret;

        if (isAccessTokenType)
        {
            jwtSecret = JwtConfiguration.AccessTokenSecretKey;
            tokenDurationInMinute = 5; // 5 minutes
        }
        else
        {
            jwtSecret = JwtConfiguration.RefreshTokenSecretKey;
            tokenDurationInMinute = 1 * 24 * 60; // 1 week
        }
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
            {
                new Claim("user_id", account.Id.ToString()),
                new Claim("user_role", account.Role.ToString()),
            };
        var token = new JwtSecurityToken(
            issuer: JwtConfiguration.Issuer,
            audience: JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenDurationInMinute),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);

    }

    public IList<Claim> GetClaims(string token)
    {
        throw new NotImplementedException();
    }

    public Guid GetCurrentUserId()
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            if (httpContextAccessor != null)
            {
                var claimsIdentity = httpContextAccessor.HttpContext?.User;
                if (claimsIdentity != null &&
                    claimsIdentity.Claims.Count() != 0 &&
                    Guid.TryParse(claimsIdentity.Claims.First(c => c.Type == "user_id").Value, out Guid userId))
                    return userId;
            }
        }
        return Guid.Empty;
    }

    //TODO: CHECK USER STATUS FROM STORAGE
    public bool ValidateToken(string token, TokenType tokenType, string[] roles)
    {
        try
        {
            Guid userId = Guid.Empty;
            string userRole = string.Empty;
            var tokenHandler = new JwtSecurityTokenHandler();
            bool isAccessTokenType = tokenType == TokenType.ACCESS_TOKEN;

            if (token == null || token == string.Empty)
                throw new UnauthorizeException("Unauthorized");
            if (!token.StartsWith("Bearer "))
                throw new BadRequestException("Missing Bearer or wrong type");

            token = token.Substring("Bearer ".Length);
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtConfiguration.Issuer,
                ValidAudience = JwtConfiguration.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(isAccessTokenType ? JwtConfiguration.AccessTokenSecretKey : JwtConfiguration.RefreshTokenSecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id") ?? throw new BadRequestException("Cannot get user id from jwt");
            userId = Guid.Parse(userIdClaim.Value);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_role") ?? throw new BadRequestException("Cannot get user role from jwt");

            userRole = roleClaim.Value;
            if (userRole == string.Empty)
                throw new BadRequestException("Cannot get user role from jwt");

            if (!roles.Contains(userRole))
                throw new UnauthorizeException("Unauthorized");

            if (userId == Guid.Empty)
                throw new BadRequestException("Cannot get user id from jwt");
            AddClaimToUserContext(jwtToken.Claims);
            return true;
        }
        catch (SecurityTokenValidationException ex)
        {
            logger.Error(ex.Message, ex);
            throw new UnauthorizeException("Unauthorized");
        }
    }

    private void AddClaimToUserContext(IEnumerable<Claim> claims)
    {
        var scope = serviceProvider.CreateScope();
        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>() ?? throw new NullReferenceException($"Null object of {nameof(IHttpContextAccessor)} type");
        if (httpContextAccessor.HttpContext != null)
        {
            httpContextAccessor.HttpContext.Items["user_claims"] = claims;
            httpContextAccessor.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
        }
    }


}
