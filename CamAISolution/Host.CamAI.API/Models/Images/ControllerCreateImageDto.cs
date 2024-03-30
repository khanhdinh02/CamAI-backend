using System.ComponentModel.DataAnnotations;
using Core.Domain.DTO;
using Host.CamAI.API.Attributes;
using Host.CamAI.API.Utils;

namespace Host.CamAI.API.Models;

public class ControllerCreateImageDto
{
    [Required, FileSizeLimit(1, SizeUnit.MB)]
    public IFormFile File { get; set; } = null!;

    public async Task<CreateImageDto> ToCreateImageDto()
    {
        var createImageDto = new CreateImageDto { ContentType = File.ContentType, Filename = File.FileName };
        using var item = new MemoryStream();
        await File.CopyToAsync(item);
        createImageDto.ImageBytes = ImageHelper.TryCompressImage(item.ToArray());
        return createImageDto;
    }
}
