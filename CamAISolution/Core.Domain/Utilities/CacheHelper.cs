using Core.Domain.Enums;

namespace Core.Domain.Utilities;

public static class CacheHelper
{
    public static string GenerateCachedKey(TokenType type, Guid userId)
    {
        return type switch
        {
            TokenType.WebAccessToken => $"WebAccessToken{userId:N}".ToLower(),
            TokenType.WebRefreshToken => $"WebRefreshToken{userId:N}".ToLower(),
            TokenType.MobileAccessToken => $"MobileAccessToken{userId:N}".ToLower(),
            TokenType.MobileRefreshToken => $"MobileRefreshToken{userId:N}".ToLower(),
            _ => throw new NotSupportedException("Type of token is not support")
        };
    }
}
