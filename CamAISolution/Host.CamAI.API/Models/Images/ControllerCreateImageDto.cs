using System.ComponentModel.DataAnnotations;
using Core.Domain.DTO;

namespace Host.CamAI.API.Models.Images;

public class ControllerCreateImageDto
{
    [Required]
    public IFormFile File { get; set; } = null!;
    public int ImageTypeId { get; set; }

    public async Task<CreateImageDto> ToCreateImageDto()
    {
        var createImageDto = new CreateImageDto
        {
            ContentType = File.ContentType,
            ImageTypeId = ImageTypeId,
            Filename = File.FileName
        };
        using var item = new MemoryStream();
        await File.CopyToAsync(item);
        createImageDto.ImageBytes = item.ToArray();
        return createImageDto;
    }
}
