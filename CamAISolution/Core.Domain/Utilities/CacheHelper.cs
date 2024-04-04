using Core.Domain.Enums;

namespace Core.Domain.Utilities;

public static class CacheHelper
{
    public static string GenerateAuthCachedKey(string userIp, TokenType type, Guid userId)
    {
        return type switch
        {
            TokenType.WebAccessToken => $"{userIp}WebAccessToken{userId:N}".ToLower(),
            TokenType.WebRefreshToken => $"{userIp}WebRefreshToken{userId:N}".ToLower(),
            TokenType.MobileAccessToken => $"{userIp}MobileAccessToken{userId:N}".ToLower(),
            TokenType.MobileRefreshToken => $"{userIp}MobileRefreshToken{userId:N}".ToLower(),
            _ => throw new NotSupportedException("Type of token is not support")
        };
    }
}
