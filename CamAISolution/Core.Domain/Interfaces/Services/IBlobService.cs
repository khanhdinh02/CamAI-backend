using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface IBlobService
{
    Task<Image> UploadImage(CreateImageDto dto, params string[] paths);

    /// <summary>
    /// Store image to file system
    /// </summary>
    /// <param name="filename">Image's filename must include extension</param>
    /// <param name="imageBytes">Image data as byte array</param>
    /// <param name="paths">Segments of path in order to store image (eg: paths = [image, must, be, store, here], result will be "$base/image/must/be/store/here/")</param>
    /// <returns>
    /// Destination of stored image.
    /// </returns>
    Task<string> StoreImageToFileSystem(string filename, byte[] imageBytes, params string[] paths);
    Task<Image> GetImageById(Guid id);

    Task DeleteImageInFilesystem(Guid id);
}
