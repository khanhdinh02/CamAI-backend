using System.ComponentModel.DataAnnotations;
using Core.Domain.Services;
using Host.CamAI.API.Models.Images;
using Host.CamAI.API.Utils;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController(IBlobService blobService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ConsumeImage(ControllerCreateImageDto dto)
    {
        return Ok(await blobService.UploadImage(await dto.ToCreateImageDto(), "test"));
    }

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
