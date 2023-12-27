using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
using Core.Domain.Models.DTO.Accounts;
using Core.Domain.Models.DTO.Auths;
using Core.Domain.Services;
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

    public string GenerateToken(Guid userID, ICollection<Role> roles, TokenTypeEnum tokenType)
    {
        int tokenDurationInMinute;
        string jwtSecret;

        if (tokenType == TokenTypeEnum.AccessToken)
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
        var claims = new List<Claim> { new("id", userID.ToString()), new("roles", JsonSerializer.Serialize(roles.Select(r => new { r.Id, r.Name }))), };

        var token = new JwtSecurityToken(
            issuer: JwtConfiguration.Issuer,
            audience: JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenDurationInMinute),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public IEnumerable<Claim> GetClaims(string token, TokenTypeEnum tokenType)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (token.IsNullOrEmpty())
                throw new UnauthorizedException("Unauthorized");

            var secretKey =
                tokenType == TokenTypeEnum.AccessToken
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
    public TokenDetailDto ValidateToken(string token, TokenTypeEnum tokenType, int[]? acceptableRoles = null)
    {
        IEnumerable<Claim> tokenClaims = this.GetClaims(token, tokenType);

        var userId = tokenClaims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new BadRequestException("Cannot get user id from jwt");

        var userRoleString = tokenClaims.FirstOrDefault(c => c.Type == "roles")?.Value;
        if (string.IsNullOrEmpty(userRoleString))
            throw new BadRequestException("Cannot get user role from jwt");
        int[] userRoles = (JsonSerializer.Deserialize<Role[]>(userRoleString) ?? []).Select(r => r.Id).ToArray();

        if (acceptableRoles != null && !acceptableRoles.Intersect(userRoles).Any())
            throw new UnauthorizedException("Unauthorized");

        AddClaimToUserContext(tokenClaims);

        return new TokenDetailDto
        {
            Token = token,
            TokenType = tokenType,
            UserId = new Guid(userId),
            UserRoles = userRoles
        };
    }

    private void AddClaimToUserContext(IEnumerable<Claim> claims)
    {
        using var scope = serviceProvider.CreateScope();
        var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
        if (httpContextAccessor == null)
        {
            logger.Info($"Null object of {nameof(IHttpContextAccessor)} type");
            throw new BaseException();
        }
        httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity(claims));
    }
}
