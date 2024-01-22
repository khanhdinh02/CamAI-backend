namespace Core.Domain.DTO;

public class DistrictDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ProvinceId { get; set; }
    public ProvinceDto Province { get; set; } = null!;
}
