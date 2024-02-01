using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface IBlobService
{
    Task<Image> UploadImage(CreateImageDto dto, params string[] paths);

    /// <summary>
    /// Store image to file system
    /// </summary>
    /// <param name="destination">physical path to store image (e.g: /some/folder/to/store/my-image.png)</param>
    /// <param name="imageBytes">Image data as byte array</param>
    /// <returns>
    /// Destination of stored image.
    /// </returns>
    Task StoreImageToFileSystem(string destination, byte[] imageBytes);
    Task<Image> GetImageById(Guid id);

    Task DeleteImageInFilesystem(Guid id);
}
