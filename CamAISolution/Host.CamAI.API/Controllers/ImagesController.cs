using Core.Domain.DTO;
using Host.CamAI.API.Models.Images;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ConsumeImage(ControllerCreateImageDto dto)
    {
        var parent = Directory.GetCurrentDirectory();
        var des = parent.Substring(0, parent.LastIndexOf('\\'));
        var folder = Path.Combine(des, "Images");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        var path = Path.Combine(
            folder,
            $"{Guid.NewGuid()}.{dto.File.FileName.Substring(dto.File.FileName.LastIndexOf('.') + 1)}"
        );
        using (var stream = System.IO.File.Create(path))
        {
            await dto.File.CopyToAsync(stream);
        }
        return Ok();
    }

    [HttpGet("{id}")]
    public Task<FileContentResult> GetImage(Guid id)
    {
        var parent = Directory.GetCurrentDirectory();
        var des = parent.Substring(0, parent.LastIndexOf('\\'));
        var folder = Path.Combine(des, "Images");
        var fileName = Directory.GetFiles(folder, $"{id}*", SearchOption.TopDirectoryOnly).First();
        return Task.FromResult(File(System.IO.File.ReadAllBytes(fileName), "image/jpeg"));
    }
}
