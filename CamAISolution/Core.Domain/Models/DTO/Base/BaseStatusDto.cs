namespace Core.Domain.DTO;

public class BaseStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
