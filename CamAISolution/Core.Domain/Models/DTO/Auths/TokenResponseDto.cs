namespace Core.Domain.Models.DTO;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
}
