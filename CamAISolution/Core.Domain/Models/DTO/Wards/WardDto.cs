namespace Core.Domain.DTO;

public class WardDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DistrictId { get; set; }
    public DistrictDto District { get; set; } = null!;
}
