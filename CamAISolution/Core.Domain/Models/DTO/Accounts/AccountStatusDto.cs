namespace Core.Domain.DTO;

public class AccountStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public byte[]? Timestamp { get; set; }
}
