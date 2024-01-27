namespace Core.Domain.Models.DTO;

public class ImageDto
{
    public Uri Uri { get; set; } = null!;
    public string ContentType { get; set; } = "image/jpeg";
}
