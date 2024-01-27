namespace Core.Domain.DTO;

public class CreateImageDto
{
    public byte[] ImageBytes { get; set; } = null!;
    public string Filename { get; set; } = null!;
    public string ContentType { get; set; } = string.Empty;
    public int ImageTypeId { get; set; }
}
