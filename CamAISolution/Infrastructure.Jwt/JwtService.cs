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

public class JwtService(
    IOptions<JwtConfiguration> configuration,
    IServiceProvider serviceProvider,
    IAppLogging<JwtService> logger
) : IJwtService
{
    private readonly JwtConfiguration JwtConfiguration = configuration.Value;

    public string GenerateToken(Account account, TokenType tokenType)
    {
        int tokenDurationInMinute;
        string jwtSecret;

        if (tokenType == TokenType.AccessToken)
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
        var claims = new List<Claim> { new("user_id", account.Id.ToString()), new("user_role", account.Role), };
        var token = new JwtSecurityToken(
            issuer: JwtConfiguration.Issuer,
            audience: JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenDurationInMinute),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public IList<Claim> GetClaims(string token)
    {
        throw new NotImplementedException();
    }

    public Guid GetCurrentUserId()
    {
        using var scope = serviceProvider.CreateScope();
        var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();

        var claimsIdentity = httpContextAccessor?.HttpContext?.User;
        if (
            claimsIdentity != null
            && claimsIdentity.Claims.Any()
            && Guid.TryParse(claimsIdentity.Claims.First(c => c.Type == "user_id").Value, out var userId)
        )
            return userId;
        return Guid.Empty;
    }

    //TODO: CHECK USER STATUS FROM STORAGE
    public bool ValidateToken(string token, TokenType tokenType, string[] roles)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (token.IsNullOrEmpty())
                throw new UnauthorizedException("Unauthorized");
            if (!token.StartsWith("Bearer "))
                throw new BadRequestException("Missing Bearer or wrong type");

            token = token.Substring("Bearer ".Length);
            var secretKey =
                tokenType == TokenType.AccessToken
                    ? JwtConfiguration.AccessTokenSecretKey
                    : JwtConfiguration.RefreshTokenSecretKey;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtConfiguration.Issuer,
                ValidAudience = JwtConfiguration.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId =
                jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value
                ?? throw new BadRequestException("Cannot get user id from jwt");

            if (userId == string.Empty)
                throw new BadRequestException("Cannot get user id from jwt");

            var userRole =
                jwtToken.Claims.FirstOrDefault(c => c.Type == "user_role")?.Value
                ?? throw new BadRequestException("Cannot get user role from jwt");

            if (userRole == string.Empty)
                throw new BadRequestException("Cannot get user role from jwt");

            if (!roles.Contains(userRole))
                throw new UnauthorizedException("Unauthorized");

            AddClaimToUserContext(jwtToken.Claims);
            return true;
        }
        catch (SecurityTokenValidationException ex)
        {
            logger.Error(ex.Message, ex);
            throw new UnauthorizedException("Unauthorized");
        }
    }

    private void AddClaimToUserContext(IEnumerable<Claim> claims)
    {
        using var scope = serviceProvider.CreateScope();
        var httpContextAccessor =
            scope.ServiceProvider.GetService<IHttpContextAccessor>()
            ?? throw new NullReferenceException($"Null object of {nameof(IHttpContextAccessor)} type");
        httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity(claims));
    }
}
