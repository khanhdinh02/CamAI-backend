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

    //TODO [Dat]: return some default image instead of throw exception

    public async Task<Image> GetImageById(Guid id)
    {
        return await imageRepo.GetByIdAsync(id) ?? throw new NotFoundException(typeof(Image), id);
    }

    public async Task<string> StoreImageToFileSystem(string filename, byte[] imageBytes, params string[] paths)
    {
        var destinationFolder = Path.Combine(paths);
        var storeFolder = Path.Combine(imgConfig.BaseImageFolderPath, destinationFolder);
        if (!Directory.Exists(storeFolder))
            Directory.CreateDirectory(storeFolder);
        var fullPhysicalPath = Path.Combine(storeFolder, filename);
        using var file = File.Create(fullPhysicalPath);
        await file.WriteAsync(imageBytes);
        file.Close();
        return fullPhysicalPath;
    }

    //TODO [Dat]: Remove hardcode part
    private Uri GenerateHostingUri(string name) => new Uri($"https://localhost:7113/api/images/{name}");

    //TODO [Dat]: Add Mapping
    public async Task<Image> UploadImage(CreateImageDto dto, params string[] paths)
    {
        var imageEntity = new Image();
        var extension = dto.Filename.Substring(dto.Filename.LastIndexOf('.'));
        var filename = $"{imageEntity.Id}{extension}";
        var uri = GenerateHostingUri(imageEntity.Id.ToString());
        var physicalPath = await StoreImageToFileSystem(filename, dto.ImageBytes, paths);
        imageEntity.PhysicalPath = physicalPath;
        imageEntity.HostingUri = uri;
        imageEntity.ContentType = dto.ContentType;
        await imageRepo.AddAsync(imageEntity);
        await unitOfWork.CompleteAsync();
        return imageEntity;
    }

    public async Task DeleteImageInFilesystem(Guid id)
    {
        var img = await imageRepo.GetByIdAsync(id);
        if (File.Exists(img!.PhysicalPath))
            File.Delete(img!.PhysicalPath);
    }
}
