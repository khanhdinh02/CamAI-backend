using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class TokenDetailDto
{
    public Guid UserId { get; set; } = Guid.Empty;

    public Role UserRole { get; set; }

    public TokenType TokenType { get; set; }

    public string? Token { get; set; }
}
