using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Core.Domain.Models.DTOs.Auths;
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

    public string GenerateToken(Guid userID, string[] roles, TokenType tokenType)
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
        var claims = new List<Claim>
        {
            new("user_id", userID.ToString()),
            new("user_role", JsonSerializer.Serialize(roles)),
        };

        var token = new JwtSecurityToken(
            issuer: JwtConfiguration.Issuer,
            audience: JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenDurationInMinute),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public IEnumerable<Claim> GetClaims(string token, TokenType tokenType)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (token.IsNullOrEmpty())
                throw new UnauthorizedException("Unauthorized");

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
            return jwtToken.Claims;
        }
        catch (SecurityTokenValidationException ex)
        {
            logger.Error(ex.Message, ex);
            throw new UnauthorizedException("Unauthorized");
        }
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
    public TokenDetailDto ValidateToken(string token, TokenType tokenType, string[]? acceptableRoles)
    {
        IEnumerable<Claim> tokenClaims = this.GetClaims(token, tokenType);

        var userId = tokenClaims.FirstOrDefault(c => c.Type == "user_id")?.Value;
        var userRoleString = tokenClaims.FirstOrDefault(c => c.Type == "user_role")?.Value;

        if (userId.IsNullOrEmpty())
            throw new BadRequestException("Cannot get user id from jwt");

        if (userRoleString.IsNullOrEmpty())
            throw new BadRequestException("Cannot get user role from jwt");

        string[] userRoles = JsonSerializer.Deserialize<string[]>(userRoleString) ?? [];

        if (acceptableRoles != null && !acceptableRoles.Intersect(userRoles).Any())
            throw new UnauthorizedException("Unauthorized");

        this.AddClaimToUserContext(tokenClaims);

        return new TokenDetailDto
        {
            Token = token,
            TokenType = tokenType,
            UserId = new Guid(userId),
            UserRoles = userRoles
        };
    }

    /*public bool ValidateTokens(List<TokenDetailDTO> tokenDetails, string[]? acceptableRoles)
    {
        Guid previousUserId = Guid.Empty;
        foreach (var tokenDTO in tokenDetails)
        {
            Guid userId = this.ValidateToken(tokenDTO, acceptableRoles);
            if (previousUserId != Guid.Empty && !previousUserId.Equals(userId))
            {
                return false;
            }
            previousUserId = userId;
        }
        return true;
    }*/

    private void AddClaimToUserContext(IEnumerable<Claim> claims)
    {
        using var scope = serviceProvider.CreateScope();
        var httpContextAccessor =
            scope.ServiceProvider.GetService<IHttpContextAccessor>()
            ?? throw new NullReferenceException($"Null object of {nameof(IHttpContextAccessor)} type");
        httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity(claims));
    }
}
