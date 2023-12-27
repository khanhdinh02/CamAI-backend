namespace Core.Domain.Models.Configurations;

public class JwtConfiguration
{
    public string AccessTokenSecretKey { get; set; } = string.Empty;
    public string RefreshTokenSecretKey { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int Expired { get; set; }
}
