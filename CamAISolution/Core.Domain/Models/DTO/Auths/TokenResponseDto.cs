namespace Core.Domain.Models.DTO.Auths;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
}
