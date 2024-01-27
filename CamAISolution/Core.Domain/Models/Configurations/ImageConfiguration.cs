namespace Core.Domain.Models.Configurations;

public class ImageConfiguration
{
    public string BaseImageFolderPath { get; set; } = null!;
    public int MaxImageSize { get; set; } // Maximum size of image
}
