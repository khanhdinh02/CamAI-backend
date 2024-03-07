using System.Security.Cryptography;

namespace Core.Domain.Utilities;

public static class RandomGenerator
{
    private const string Alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string GetAlphanumericString(int length)
    {
        return RandomNumberGenerator.GetString(Alphanumeric.AsSpan(), length);
    }
}
