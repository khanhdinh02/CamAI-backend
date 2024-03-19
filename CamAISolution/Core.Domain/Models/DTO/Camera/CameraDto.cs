using Core.Domain.DTO;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class CameraDto : BaseDto
{
    public string Name { get; set; } = null!;
    public Guid ShopId { get; set; }
    public Zone Zone { get; set; }
    public CameraStatus Status { get; set; }
}
