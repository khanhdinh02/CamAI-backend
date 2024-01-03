using Core.Domain.DTO;

namespace Core.Domain.DTO;

public class TokenDetailDto
{
    public Guid UserId { get; set; } = Guid.Empty;

    public int[] UserRoles { get; set; } = [];

    public TokenTypeEnum TokenType { get; set; }

    public string? Token { get; set; }
}
