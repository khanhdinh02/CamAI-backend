using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
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

    public string GenerateToken(Guid userId, Role role, TokenType tokenType) =>
        GenerateToken(userId, role, null, tokenType);

    public string GenerateToken(Guid userId, Role role, AccountStatus? status, TokenType tokenType)
    {
        var claims = new List<Claim> { new("id", userId.ToString()), new("role", role.ToString()) };

        int tokenDurationInMinute;
        string jwtSecret;
        if (tokenType == TokenType.AccessToken)
        {
            jwtSecret = jwtConfiguration.AccessToken.Secret;
            tokenDurationInMinute = jwtConfiguration.AccessToken.Duration;
            if (status != null)
                claims.Add(new Claim("status", status.ToString()!));
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

    public IEnumerable<Claim> GetClaims(string token, TokenType tokenType, bool isValidateTime = true)
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
    public TokenDetailDto ValidateToken(
        string token,
        TokenType tokenType,
        Role[]? acceptableRoles = null,
        bool isValidateTime = true
    )
    {
        IEnumerable<Claim> tokenClaims = GetClaims(token, tokenType, isValidateTime);

        var userId = tokenClaims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new BadRequestException("Cannot get user id from jwt");

        var userRoleString = tokenClaims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (!Enum.TryParse<Role>(userRoleString, true, out var userRole))
            throw new BadRequestException("Cannot get user role from jwt");

        if (acceptableRoles is { Length: > 0 } && !acceptableRoles.Contains(userRole))
            throw new ForbiddenException("Unauthorized");

        AddClaimToUserContext(tokenClaims);

        return new TokenDetailDto
        {
            Token = token,
            TokenType = tokenType,
            UserId = new Guid(userId),
            UserRole = userRole
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
