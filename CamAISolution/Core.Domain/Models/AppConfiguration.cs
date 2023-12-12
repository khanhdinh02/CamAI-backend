namespace Core.Domain.Models;
public class AppConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public string JwtSecret { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int Expired { get; set; }
}