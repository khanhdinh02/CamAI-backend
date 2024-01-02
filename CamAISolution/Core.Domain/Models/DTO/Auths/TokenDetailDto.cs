using Core.Domain.Models.DTO.Accounts;

namespace Core.Domain.Models.DTO.Auths;

public class TokenDetailDto
{
    public Guid UserId { get; set; } = Guid.Empty;

    public int[] UserRoles { get; set; } = [];

    public TokenTypeEnum TokenType { get; set; }

    public string? Token { get; set; }
}
