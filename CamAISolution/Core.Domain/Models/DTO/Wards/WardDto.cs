namespace Core.Domain.DTO;

public class WardDto : BaseDto
{
    public string Name { get; set; } = null!;
    public Guid DistrictId { get; set; }
}
