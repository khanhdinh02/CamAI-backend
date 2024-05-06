using Core.Domain.Enums;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController(ILogger<FilesController> logger) : ControllerBase
{
    /// <summary>
    /// Brand and shop manager only
    /// </summary>
    /// <returns></returns>
    [HttpGet("download/employee-csv")]
    [AccessTokenGuard(Role.BrandManager, Role.ShopManager)]

    public IActionResult DownloadEmployeeCsvTemplate()
    {
        var filename = Directory.EnumerateFiles(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf('/')), "EmployeeTemplate.csv", SearchOption.AllDirectories).First();
        logger.LogInformation("Searched files: {Parameter}", filename);
        using var file = System.IO.File.OpenRead(filename);
        var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream, "text/csv", "EmployeeTemplate.csv");
    }

    /// <summary>
    /// Brand manager only
    /// </summary>
    /// <returns></returns>
    [HttpGet("download/shop-csv")]
    [AccessTokenGuard(Role.BrandManager)]
    public IActionResult DownloadShopCsvTemplate()
    {
        var filename = Directory.EnumerateFiles(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf('/')), "ShopTemplate.csv", SearchOption.AllDirectories).First();
        logger.LogInformation("Searched files: {Parameter}", filename);
        using var file = System.IO.File.OpenRead(filename);
        var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream, "text/csv", "ShopTemplate.csv");
    }
}