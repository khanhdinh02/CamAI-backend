using System.ComponentModel.DataAnnotations;
using Core.Domain.Services;
using Host.CamAI.API.Utils;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AccessTokenGuard]
public class ImagesController(IBlobService blobService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<FileStreamResult> GetImage(
        Guid id,
        int? width = null,
        int? height = null,
        [Range(0, 2)] float scaleFactor = 1
    )
    {
        var img = await blobService.GetImageById(id);
        return File(ImageHelper.Resize(img.PhysicalPath, width, height, scaleFactor), img.ContentType);
    }
}
