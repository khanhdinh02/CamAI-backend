using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class BloblService(IUnitOfWork unitOfWork, ImageConfiguration imgConfig) : IBlobService
{
    private readonly IRepository<Image> imageRepo = unitOfWork.GetRepository<Image>();

    //TODO [Dat]: return some default image instead of throw exception
    public async Task<byte[]> GetImage(string path)
    {
        if (File.Exists(path))
        {
            using var ms = new MemoryStream();
            using var file = File.OpenRead(path);
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
        else
            throw new NotFoundException("Image not found");
    }

    public async Task<string> StoreImageToFileSystem(string filename, byte[] imageBytes, params string[] paths)
    {
        var destinationFolder = Path.Combine(paths);
        var fullPhysicalPath = Path.Combine(imgConfig.BaseImageFolderPath, destinationFolder, filename);
        using var file = File.Create(fullPhysicalPath);
        await file.WriteAsync(imageBytes);
        file.Close();
        return fullPhysicalPath;
    }

    //TODO [Dat]: Remove hardcode part
    private Uri GenerateHostingUri(string name) => new Uri($"http://localhost:7133/api/images/{name}");

    //TODO [Dat]: Add Mapping
    public async Task<Image> UploadImage(CreateImageDto dto)
    {
        var imageEntity = new Image();
        var extension = dto.Filename.Substring(dto.Filename.LastIndexOf('.'));
        var filename = $"{imageEntity.Id}{extension}";
        var uri = GenerateHostingUri(filename);
        var physicalPath = await StoreImageToFileSystem(filename, dto.ImageBytes);
        imageEntity.PhysicalPath = physicalPath;
        imageEntity.HostingUri = uri;
        imageEntity.ContentType = dto.ContentType;
        imageEntity.ImageTypeId = dto.ImageTypeId;
        await imageRepo.AddAsync(imageEntity);
        await unitOfWork.CompleteAsync();
        return imageEntity;
    }
}
