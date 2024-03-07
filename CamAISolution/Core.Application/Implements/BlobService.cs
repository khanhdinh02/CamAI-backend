using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class BlobService(IUnitOfWork unitOfWork, ImageConfiguration imgConfig) : IBlobService
{
    private readonly IRepository<Image> imageRepo = unitOfWork.GetRepository<Image>();

    public async Task<Image> GetImageById(Guid id)
    {
        return await imageRepo.GetByIdAsync(id) ?? throw new NotFoundException(typeof(Image), id);
    }

    public async Task StoreImageToFileSystem(string destination, byte[] imageBytes)
    {
        using var file = File.Create(destination);
        await file.WriteAsync(imageBytes);
    }

    private string CreatePhysicalPath(string filename, params string[] paths)
    {
        var destinationFolder = Path.Combine(paths);
        var storeFolder = Path.Combine(imgConfig.BaseImageFolderPath, destinationFolder);
        if (!Directory.Exists(storeFolder))
            Directory.CreateDirectory(storeFolder);
        return Path.Combine(storeFolder, filename);
    }

    private Uri GenerateHostingUri(string name) => new Uri($"{imgConfig.HostingUri}/{name}");

    public async Task<Image> UploadImage(CreateImageDto dto, params string[] paths)
    {
        var imageEntity = new Image { Id = Guid.NewGuid() };
        var extension = Domain.Utilities.FileHelper.GetExtension(dto.Filename);
        var filename = $"{imageEntity.Id}{extension}";
        var uri = GenerateHostingUri(imageEntity.Id.ToString());
        var physicalPath = CreatePhysicalPath(filename, paths);
        imageEntity.PhysicalPath = physicalPath;
        imageEntity.HostingUri = uri;
        imageEntity.ContentType = dto.ContentType;
        await imageRepo.AddAsync(imageEntity);
        if (await unitOfWork.CompleteAsync() > 0)
            await StoreImageToFileSystem(imageEntity.PhysicalPath, dto.ImageBytes);
        return imageEntity;
    }

    public async Task DeleteImageInFilesystem(Guid id)
    {
        var img = await imageRepo.GetByIdAsync(id);
        if (img == null)
            return;
        imageRepo.Delete(img);

        if (await unitOfWork.CompleteAsync() > 0 && File.Exists(img!.PhysicalPath))
            File.Delete(img!.PhysicalPath);
    }
}
