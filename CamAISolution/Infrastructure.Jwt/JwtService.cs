using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static Core.Domain.Utilities.CacheHelper;

namespace Infrastructure.Jwt;

public class JwtService(
    IOptions<JwtConfiguration> configuration,
    IServiceProvider serviceProvider,
    IAppLogging<JwtService> logger
) : IJwtService
{
    private readonly JwtConfiguration jwtConfiguration = configuration.Value;
    private Account? currentUser;

    public string GenerateToken(Guid userId, Role role, TokenType tokenType, string userIp) =>
        GenerateToken(userId, role, null, tokenType, userIp);

    public string GenerateToken(Guid userId, Role role, AccountStatus? status, TokenType tokenType, string userIp)
    {
        var claims = new List<Claim> { new("id", userId.ToString()), new("role", role.ToString()) };

        int tokenDurationInMinute;
        string jwtSecret;
        if (tokenType is TokenType.WebAccessToken or TokenType.MobileAccessToken)
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
        using var scope = serviceProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        cacheService.Set(
            $"{userIp}{GenerateCachedKey(tokenType, userId)}",
            tokenStr,
            TimeSpan.FromMinutes(tokenDurationInMinute)
        );
        return tokenStr;
    }

    public IEnumerable<Claim> GetClaims(string token, TokenType tokenType, bool isValidateTime = true)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (token.IsNullOrEmpty())
                throw new UnauthorizedException("Unauthorized");

            var secretKey = tokenType is TokenType.WebAccessToken or TokenType.MobileAccessToken
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

    public async Task SetCurrentUserToSystemHandler()
    {
        //TODO[Dat] : get system handler by role
        using var scope = serviceProvider.CreateScope();
        currentUser = (
            await scope
                .ServiceProvider.GetRequiredService<IUnitOfWork>()
                .Accounts.GetAsync(x => x.Email == "systemhandler")
        ).Values[0];
    }

    public Account GetCurrentUser()
    {
        if (currentUser != null)
            return currentUser;

        using var scope = serviceProvider.CreateScope();
        var httpContext =
            scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext
            ?? throw new ServiceUnavailableException("Service Unavailable");
        currentUser = httpContext.Items[nameof(Account)] as Account ?? throw new UnauthorizedException("");
        return currentUser;
    }

    //TODO: CHECK USER STATUS FROM STORAGE
    public TokenDetailDto ValidateToken(
        string token,
        TokenType tokenType,
        string userIp,
        Role[]? acceptableRoles = null,
        bool isValidateTime = true
    )
    {
        IEnumerable<Claim> tokenClaims = GetClaims(token, tokenType, isValidateTime);

        var userId = tokenClaims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new BadRequestException("Cannot get user id from jwt");

        ValidateTokenInCacheMemory(token, tokenType, Guid.Parse(userId), userIp);

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

    private void ValidateTokenInCacheMemory(string token, TokenType tokenType, Guid userId, string userIp)
    {
        using var scope = serviceProvider.CreateScope();
        var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
        string key = $"{userIp}{GenerateCachedKey(tokenType, userId)}";
        var cacheToken = cacheService.Get<string>(key);
        if (cacheToken == null || (cacheToken != null && cacheToken != token))
            throw new BadRequestException("Token is invalid");
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
