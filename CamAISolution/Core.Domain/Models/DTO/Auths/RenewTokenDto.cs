namespace Core.Domain.Models.DTO.Auths;

public class RenewTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
