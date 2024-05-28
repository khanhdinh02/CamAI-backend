using System.ComponentModel.DataAnnotations;
using Core.Domain.Services;
using Host.CamAI.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController(IBlobService blobService, ILogger<ImagesController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<FileStreamResult> GetImage(
        Guid id,
        int? width = null,
        int? height = null,
        [Range(0, 1)] float scaleFactor = 1
    )
    {
        try
        {
            var img = await blobService.GetImageById(id);
            return File(
                Infrastructure.Blob.ImageHelper.Resize(img.PhysicalPath, width, height, scaleFactor),
                img.ContentType
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return File(new MemoryStream(), "image/png");
        }
    }
}
