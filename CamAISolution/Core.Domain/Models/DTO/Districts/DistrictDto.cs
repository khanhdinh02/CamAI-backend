using Core.Domain.DTO;

namespace Core.Domain.Models.DTO;

public class DistrictDto : BaseDto
{
    public string Name { get; set; } = null!;
    public Guid ProvinceId { get; set; }

    public ProvinceDto Province { get; set; } = null!;
}
