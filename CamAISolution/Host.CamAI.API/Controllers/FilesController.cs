using Core.Domain.Enums;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    [HttpGet("download/employee-csv")]
    [AccessTokenGuard(Role.BrandManager, Role.ShopManager)]
    public IActionResult DownloadEmployeeCsvTemplate()
    {
        using var file = System.IO.File.OpenRead("../Core.Domain/Statics/EmployeeTemplate.csv");
        var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream, "text/csv", "EmployeeTemplate.csv");
    }

    [HttpGet("download/shop-csv")]
    [AccessTokenGuard(Role.BrandManager)]
    public IActionResult DownloadShopCsvTemplate()
    {
        using var file = System.IO.File.OpenRead("../Core.Domain/Statics/ShopTemplate.csv");
        var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream, "text/csv", "ShopTemplate.csv");
    }
}