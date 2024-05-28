using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Infrastructure.Blob;

public class BlobService(IUnitOfWork unitOfWork, ImageConfiguration imgConfig, IAppLogging<BlobService> logger)
    : IBlobService
{
    private readonly IRepository<Image> imageRepo = unitOfWork.GetRepository<Image>();

    private Uri GenerateHostingUri(string name) => new Uri($"{imgConfig.HostingUri}/{name}");

    private string CreatePhysicalPath(string filename, params string[] paths)
    {
        var destinationFolder = Path.Combine(paths);
        var storeFolder = Path.Combine(imgConfig.BaseImageFolderPath, destinationFolder);
        if (!Directory.Exists(storeFolder))
            Directory.CreateDirectory(storeFolder);
        return Path.Combine(storeFolder, filename);
    }

    public async Task DeleteImageInFilesystem(Guid id)
    {
        logger.Warn("Delete image");
        var img = await imageRepo.GetByIdAsync(id);
        if (img == null)
            return;
        imageRepo.Delete(img);

        if (await unitOfWork.CompleteAsync() > 0 && File.Exists(img!.PhysicalPath))
            File.Delete(img!.PhysicalPath);
    }

    public async Task<Image> GetImageById(Guid id) =>
        await imageRepo.GetByIdAsync(id) ?? throw new NotFoundException(typeof(Image), id);

    public async Task StoreImageToFileSystem(string destination, byte[] imageBytes)
    {
        logger.Info("Write image to file system");
        using var file = File.Create(destination);
        await file.WriteAsync(imageBytes);
    }

    public async Task<Image> UploadImage(CreateImageDto dto, params string[] paths)
    {
        logger.Info("Upload image");
        var imageEntity = new Image { Id = Guid.NewGuid() };
        var extension = Core.Domain.Utilities.FileHelper.GetExtension(dto.Filename);
        var filename = $"{imageEntity.Id}{extension}";
        var uri = GenerateHostingUri(imageEntity.Id.ToString());
        var physicalPath = CreatePhysicalPath(filename, paths);
        imageEntity.PhysicalPath = physicalPath;
        imageEntity.HostingUri = uri;
        imageEntity.ContentType = dto.ContentType;
        await imageRepo.AddAsync(imageEntity);
        if (await unitOfWork.CompleteAsync() > 0)
        {
            try
            {
                await StoreImageToFileSystem(imageEntity.PhysicalPath, ImageHelper.TryCompressImage(dto.ImageBytes));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                imageRepo.Delete(imageEntity);
                await unitOfWork.CompleteAsync();
            }
        }
        return imageEntity;
    }
}
