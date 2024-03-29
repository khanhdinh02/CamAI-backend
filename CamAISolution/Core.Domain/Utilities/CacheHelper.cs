using Core.Domain.Enums;

namespace Core.Domain.Utilities;

public static class CacheHelper
{
    public static string GenerateCachedKey(TokenType type, Guid userId)
    {
        return type switch
        {
            TokenType.AccessToken => $"AccessToken{userId}".ToLower(),
            TokenType.RefreshToken => $"RefreshToken{userId}".ToLower(),
            _ => throw new NotSupportedException("Type of token is not support")
        };
    }
}