namespace Core.Domain;

public class ShopStatusDto : BaseDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
