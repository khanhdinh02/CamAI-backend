using Core.Domain.Models.Enums;

namespace Core.Domain.Models.DTOs.Auths;

public class TokenDetailDto
{
    public Guid UserId { get; set; } = Guid.Empty;

    public string[] UserRoles { get; set; } = [];

    public TokenType TokenType { get; set; }

    public string? Token { get; set; }
}
