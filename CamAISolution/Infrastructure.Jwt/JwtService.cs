using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
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
    private readonly JwtConfiguration jwtConfiguration = configuration.Value;

    public string GenerateToken(Guid userId, IEnumerable<Role> roles, TokenType tokenType) =>
        GenerateToken(userId, roles, null, tokenType);

    public string GenerateToken(Guid userId, IEnumerable<Role> roles, AccountStatus? status, TokenType tokenType)
    {
        var claims = new List<Claim>
        {
            new("id", userId.ToString()),
            new("roles", JsonSerializer.Serialize(roles.Select(r => new { r.Id, r.Name }))),
        };

        int tokenDurationInMinute;
        string jwtSecret;
        if (tokenType == TokenType.AccessToken)
        {
            jwtSecret = jwtConfiguration.AccessToken.Secret;
            tokenDurationInMinute = jwtConfiguration.AccessToken.Duration;
            if (status != null)
                claims.Add(new Claim("status", JsonSerializer.Serialize(new { status.Id, status.Name })));
        }
        else
        {
            jwtSecret = jwtConfiguration.RefreshToken.Secret;
            tokenDurationInMinute = jwtConfiguration.RefreshToken.Duration;
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfiguration.Issuer,
            audience: jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenDurationInMinute),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public IEnumerable<Claim> GetClaims(string token, TokenType tokenType, bool isValidateTime)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (token.IsNullOrEmpty())
                throw new UnauthorizedException("Unauthorized");

            var secretKey =
                tokenType == TokenType.AccessToken
                    ? jwtConfiguration.AccessToken.Secret
                    : jwtConfiguration.RefreshToken.Secret;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                ValidateLifetime = isValidateTime,
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

    public Account GetCurrentUser()
    {
        using var scope = serviceProvider.CreateScope();
        var httpContext =
            scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext
            ?? throw new ServiceUnavailableException("Service Unavailable");
        return (httpContext.Items[nameof(Account)] as Account) ?? throw new UnauthorizedException("");
    }

    //TODO: CHECK USER STATUS FROM STORAGE
    public TokenDetailDto ValidateToken(string token, TokenType tokenType, int[]? acceptableRoles, bool isValidateTime)
    {
        IEnumerable<Claim> tokenClaims = GetClaims(token, tokenType, isValidateTime);

        var userId = tokenClaims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new BadRequestException("Cannot get user id from jwt");

        var userRoleString = tokenClaims.FirstOrDefault(c => c.Type == "roles")?.Value;
        if (string.IsNullOrEmpty(userRoleString))
            throw new BadRequestException("Cannot get user role from jwt");
        var userRoles = (JsonSerializer.Deserialize<Role[]>(userRoleString) ?? []).Select(r => r.Id).ToArray();

        if (acceptableRoles is { Length: > 0 } && !acceptableRoles.Intersect(userRoles).Any())
            throw new ForbiddenException("Unauthorized");

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
            throw new ServiceUnavailableException("Error");
        }
        httpContextAccessor.HttpContext?.User.AddIdentity(new ClaimsIdentity(claims));
    }
}
