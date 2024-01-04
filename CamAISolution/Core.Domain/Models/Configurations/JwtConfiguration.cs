namespace Core.Domain.Models.Configurations;

public class JwtConfiguration
{
    public TokenConfiguration AccessToken { get; set; } = null!;
    public TokenConfiguration RefreshToken { get; set; } = null!;

    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int Expired { get; set; }
}

public class TokenConfiguration
{
    public string Secret { get; set; } = null!;
    public int Duration { get; set; }
}
