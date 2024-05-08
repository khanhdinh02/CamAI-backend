namespace Core.Domain.Utilities;

public static class DomainHelper
{
    public static string GenerateDefaultPassword(string email)
    {
        var emailName = email.Split('@')[0];
        return $"{emailName}@123";
    }
}