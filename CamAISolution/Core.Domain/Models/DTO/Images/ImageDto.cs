using Core.Domain.DTO;

namespace Core.Domain.Models.DTO;

public class ImageDto : BaseDto
{
    public Uri HostingUri { get; set; } = null!;
    public string ContentType { get; set; } = string.Empty;
}
