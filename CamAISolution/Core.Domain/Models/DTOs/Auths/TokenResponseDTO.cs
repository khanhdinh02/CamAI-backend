namespace Core.Domain.Models.DTOs.Auths;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
}
